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
    [HttpPost("[action]")]
    public async Task<ApiResult<IndexResDto<DepositResDto>>> Index(IndexDto dto, CancellationToken ct)
    {
        dto.Page = Math.Max(1, dto.Page);
        dto.Limit = Math.Max(10, dto.Limit);
        var query = repository.TableNoTracking
            .AsQueryable();
        
        var total = await query.CountAsync(ct);
        var list = await query
            .Skip((dto.Page - 1) * dto.Limit)
            .Take(dto.Limit)
            .ProjectTo<DepositResDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
        return Ok(new IndexResDto<DepositResDto>
        {
            Total = total,
            Data = list,
            Page = dto.Page,
            Limit = dto.Limit,
            Details = new()
        });
    }

    [HttpGet("[action]")]
    public async Task<ApiResult<List<DepositUserResDto>>> GetDepositUsers([FromQuery]int depositId, CancellationToken ct)
    {
        var result = await depositUserRepository
            .TableNoTracking
            .Where(i => i.DepositId == depositId)
            .ProjectTo<DepositUserResDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
        return Ok(result);

    }
}