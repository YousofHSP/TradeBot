using Asp.Versioning;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Exceptions;
using Data.Contracts;
using Domain.Entities;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Exchange;
using Shared;
using Shared.DTOs;
using WebFramework.Api;

namespace Api.Controllers.v1;

[ApiVersion("1")]
public class CoinController(
    IRepository<Coin> repository,
    IRepository<Role> roleRepository,
    RoleManager<Role> roleManager,
    IHashEntityValidator hashEntityValidator,
    IRepository<Deposit> depositRepository,
    IRepository<Order> orderRepository,
    IRepository<Setting> settingRepository,
    IMapper mapper,
    IExchange exchange

    )
    : CrudController<CoinDto, CoinResDto, Coin>(repository, mapper, hashEntityValidator, roleRepository, roleManager)
{
    [HttpGet]

    [HttpGet("[action]/{currency}")]
    public async Task<ApiResult<CoinResDto>> Get(string currency, CancellationToken ct)
    {
        // var dto = await exchange.GetCoin(currency, ct);
        // if (dto is null)
        //     throw new NotFoundException("ارز پیدا نشد");
        var price = await exchange.GetBalance(currency, ct);
        var dto = new CoinResDto()
        {
            Currency = currency,
            Price = price
        };
        return Ok(dto);
    }

    [HttpGet("[action]")]
    public async Task<ApiResult<List<CoinResDto>>> GetNotStored(string? currency, CancellationToken ct)
    {
        var list = await exchange.GetCoins(currency, ct);
        var storedList = await repository.TableNoTracking.ToListAsync(ct);
        list = list.Where(i => storedList.All(c => i.Id != c.Id)).ToList();
        return Ok(list);
    }

    [HttpPost("Create")]
    public override async Task<ApiResult<CoinResDto>> Create(CoinDto dto, CancellationToken cancellationToken)
    {
        var exchangeCoin = await exchange.GetCoin(dto.Currency, cancellationToken);
        if (exchangeCoin is null)
            throw new NotFoundException("ارز پیدا نشد");
        var isExists = await repository.TableNoTracking.AnyAsync(i => i.Currency == exchangeCoin.Currency, cancellationToken);
        if (isExists)
            throw new BadRequestException("این ارز قبلا ثبت شده");
        var model = exchangeCoin.ToEntity(mapper);
        model.LoseLimit = dto.LoseLimit;
        model.ProfitLimit = dto.ProfitLimit;
        await repository.AddAsync(model, cancellationToken);

        return CoinResDto.FromEntity(model, mapper);
    }
    
    [HttpPost("[action]")]
    public async Task<ApiResult> Buy(BuyCoinDto dto, CancellationToken ct)
    {
        var coin = await repository.TableNoTracking
            .FirstOrDefaultAsync(i => i.Currency == dto.Currency, ct);
        if (coin is null)
            throw new NotFoundException("ارز پیدا نشد");
        
        var deposit = await depositRepository.TableNoTracking
            .Where(d => d.EndDate == null)
            .FirstOrDefaultAsync(ct);
        if(deposit is null)
            throw new NotFoundException("سرمایه گذاری فعالی یافت نشد");

        var result = await exchange.PlaceOrder(
            coin,
            leverage: 3,
            ct);
        if (result == 0)
            return new ApiResult(false, ApiResultStatusCode.ServerError);
        var order = new Order
        {
            OrderId = result,
            Currency = dto.Currency,
        };
        await orderRepository.AddAsync(order, ct);
        
        return Ok();
    }
}