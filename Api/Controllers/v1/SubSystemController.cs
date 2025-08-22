using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using AutoMapper;
using Common.Exceptions;
using Data.Contracts;
using Domain.Entities;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using WebFramework.Api;

namespace Api.Controllers.v1
{
    [Display(Name = "زیرسیستم")]
    [ApiVersion("1")]
    public class SubSystemController : CrudController<SubSystemDto, SubSystemResDto, SubSystem>
    {
        public SubSystemController(IRepository<SubSystem> repository, IMapper mapper, IHashEntityValidator hashEntityValidator,
            IRepository<Role> roleRepository, RoleManager<Role> roleManager)
            : base(repository, mapper, hashEntityValidator, roleRepository, roleManager)
        {
        }
        protected override IQueryable<SubSystem> setIncludes(IQueryable<SubSystem> query)
        {
            query.Include(i => i.AdminUser)
                .ThenInclude(i => i.Info)
                .Include(i => i.City);
            return base.setIncludes(query);
        }

        protected override IQueryable<SubSystem> setSearch(string? search, IQueryable<SubSystem> query)
        {
            if (string.IsNullOrEmpty(search)) return query;
            query = query.Where(i => i.Title.Contains(search)
                || i.Address.Contains(search)
                || i.City.Title.Contains(search)
                || i.AdminUser.UserName.Contains(search));
            return query;
        }
        public override async Task<ApiResult<SubSystemResDto>> Create(SubSystemDto dto, CancellationToken cancellationToken)
        {

            var isExsits = await Repository.TableNoTracking.AnyAsync(i => i.AdminUserId == dto.AdminUserId);
            if (isExsits) throw new BadRequestException("برای این کاربر قبلا زیرسیستم تعریف شده");
            isExsits = await Repository.TableNoTracking.AnyAsync(i => i.PostalCode == dto.PostalCode);
            if (isExsits) throw new BadRequestException("کدپستی قبلا ثبت شده");
            isExsits = await Repository.TableNoTracking.AnyAsync(i => i.Title == dto.Title);
            if (isExsits) throw new BadRequestException("این عنوان قبلا ثبت شده");


            return await base.Create(dto, cancellationToken);
        }
    }
}
