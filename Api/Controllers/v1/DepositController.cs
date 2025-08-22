using Asp.Versioning;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using WebFramework.Api;

namespace Api.Controllers.v1;

[ApiController]
[ApiVersion("1")]
public class DepositController(
    IRepository<Deposit> repository, 
    IMapper mapper,
    IRepository<DepositUser> depositUserRepository)
    : BaseController
{
    [HttpGet]
    public async Task<ApiResult<List<DepositResDto>>> Get(CancellationToken ct)
    {
        var result = await repository.TableNoTracking
            .ProjectTo<DepositResDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
        return Ok(result);
    }

    [HttpGet("[action]")]
    public async Task<ApiResult<List<DepositUserResDto>>> GetDepositUsers(int depositId, CancellationToken ct)
    {
        var result = await depositUserRepository
            .TableNoTracking
            .Where(i => i.DepositId == depositId)
            .ProjectTo<DepositUserResDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
        return Ok(result);

    }
}