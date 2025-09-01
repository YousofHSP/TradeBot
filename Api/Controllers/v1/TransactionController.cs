using Asp.Versioning;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Exchange;
using Shared;
using Shared.DTOs;
using WebFramework.Api;

namespace Api.Controllers.v1;

[ApiVersion("1")]
public class TransactionController(
    IRepository<Transaction> transactionRepository,
    IExchange exchange,
    IRepository<Deposit> depositRepository,
    IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ApiResult<List<TransactionDto>>> Get(CancellationToken ct)
    {
        var list = await transactionRepository
            .TableNoTracking
            .ProjectTo<TransactionDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
        return Ok(list);
    }

    [HttpPost("[action]")]
    public async Task<ApiResult> SetDeposit(CancellationToken ct)
    {
        var check= await depositRepository.TableNoTracking
            .Include(i => i.DepositUsers)
            .AnyAsync(i => i.EndAmount == null, ct);
        if (check)
        {
            await EndDeposit(ct);
        }else
        {
            await StartDeposit(ct);
        }

        return Ok();

    }
    private async Task StartDeposit(CancellationToken cancellationToken)
    {
        
        var transactions = await transactionRepository.TableNoTracking
            .ToListAsync(cancellationToken);

        var amount = transactions.Sum(i => i.Amount);
        var accountAmount = await exchange.GetBalance(cancellationToken: cancellationToken);
        if (accountAmount != amount)
            throw new InvalidDataException("مقدار حساب سیستم با مقدار صرافی یکی نیست");

        var userId = User.Identity!.GetUserId<int>();
        var deposit = new Deposit()
        {
            StartDate = DateTimeOffset.Now,
            StartAmount = amount,
            CreatorUserId = userId
        };
        var depositUsers = new List<DepositUser>();
        var newTransactions = new List<Transaction>();
        foreach (var items in transactions.GroupBy(i => i.UserId))
        {
            depositUsers.Add(new DepositUser()
            {
                UserId = items.Key,
                CreateDate = DateTimeOffset.Now,
                StartAmount = transactions.Sum(i => i.Amount),
                CreatorUserId = userId
            });

            newTransactions.Add(new Transaction()
            {
                Amount = transactions.Sum(i => i.Amount) * -1,
                CreateDate= DateTimeOffset.Now,
                Type = TransactionType.Deposit,
                UserId = items.Key,
                CreatorUserId = userId
            });
        }

        deposit.DepositUsers = depositUsers;
        await depositRepository.AddAsync(deposit, cancellationToken, false);
        await transactionRepository.AddRangeAsync(newTransactions, cancellationToken);
    }

    private async Task EndDeposit(CancellationToken cancellationToken)
    {
        var deposit = await depositRepository.TableNoTracking
            .Include(i => i.DepositUsers)
            .FirstOrDefaultAsync(i => i.EndAmount == null, cancellationToken);
        if (deposit is null)
            throw new LogicException("هیچ سرمایه گذاری فعالی وجود ندارد");
        deposit.EndDate = DateTimeOffset.Now;
        deposit.EndAmount = await exchange.GetBalance(cancellationToken: cancellationToken);
        if (deposit.EndAmount is null)
            throw new AppException("خطای سمت صرافی");
        var percent = deposit.StartAmount * deposit.EndAmount / 100;
        var transactions = new List<Transaction>();
        foreach (var depositUser in deposit.DepositUsers)
        {
            var endAmount = percent * depositUser.StartAmount / 100;
            depositUser.EndAmount = endAmount.Value;
            transactions.Add(new Transaction()
            {
                UserId = depositUser.UserId,
                CreateDate = DateTimeOffset.Now,
                Type = TransactionType.EndDeposit,
                Amount = endAmount.Value
            });
        }

        await depositRepository.UpdateAsync(deposit, cancellationToken, false);
        await transactionRepository.AddRangeAsync(transactions, cancellationToken);
    }
}