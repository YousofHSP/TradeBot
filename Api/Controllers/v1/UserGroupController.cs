using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using AutoMapper;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using Domain.Entities;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using WebFramework.Api;

namespace Api.Controllers.v1;

[Display(Name = "گروه کاری")]
[ApiVersion("1")]
public class UserGroupController(
    IRepository<UserGroup> repository,
    IMapper mapper,
    IHashEntityValidator hashEntityValidator,
    IRepository<Role> roleRepository,
    RoleManager<Role> roleManager,
    IRepository<ApiToken> apiTokenRepository)
    : CrudController<UserGroupDto, UserGroupResDto, UserGroup>(repository, mapper, hashEntityValidator, roleRepository,
        roleManager)
{
    protected override IQueryable<UserGroup> setIncludes(IQueryable<UserGroup> query)
    {
        query = query.Include(i => i.Roles);
        return query;
    }

    protected override IQueryable<UserGroup> setSearch(string? search, IQueryable<UserGroup> query)
    {
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(i => i.Title.Contains(search));
        }
        return query;
    }

    public override async Task<ApiResult<UserGroupResDto>> Create(UserGroupDto dto, CancellationToken cancellationToken)
    {
        var userId = User.Identity!.GetUserId<int>();
        var model = dto.ToEntity(Mapper);
        var roles = await roleRepository.Table
            .Where(i => dto.RoleIds.Contains(i.Id))
            .AsTracking()
            .ToListAsync(cancellationToken);
        if (roles.Count == 0)
            throw new BadRequestException("نقش را انتخاب کنید");
        model.Roles = roles;
        var tokenCode = User.Identity!.FindFirstValue("TokenCode");
        var apiToken = await apiTokenRepository.TableNoTracking
            .FirstOrDefaultAsync(i => i.Code == tokenCode && i.SubSystemId != null, cancellationToken);
        if (apiToken is null)
            throw new LogicException("زیرسیستم انتخاب نشده");
        model.SubSystemId = apiToken.SubSystemId!.Value;
        model.CreatorUserId = userId;
        await Repository.AddAsync(model, cancellationToken);
        var result = UserGroupResDto.FromEntity(model, Mapper);
        return Ok(result);
    }

    public override async Task<ApiResult<UserGroupResDto>> Update(UserGroupDto dto, CancellationToken cancellationToken)
    {
        var model = await Repository.Table
            .Include(i => i.Roles)
            .AsTracking()
            .FirstOrDefaultAsync(i => i.Id == dto.Id, cancellationToken);
        if (model is null)
            throw new NotFoundException("گروه کاری پیدا نشد");
        model = dto.ToEntity(model,Mapper);
        model.Roles.Clear();
        await Repository.UpdateAsync(model, cancellationToken);
        {
            var roles = await roleRepository.Table
                .Where(i => dto.RoleIds.Contains(i.Id))
                .AsTracking()
                .ToListAsync(cancellationToken);
            if (roles.Count == 0)
                throw new BadRequestException("نقش را انتخاب کنید");

            model.Roles = roles;
        }
        await Repository.UpdateAsync(model, cancellationToken);
        var result = UserGroupResDto.FromEntity(model, Mapper);
        return Ok(result);
    }
}