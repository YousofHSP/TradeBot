using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using AutoMapper;
using Data.Contracts;
using Domain.Entities;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using WebFramework.Api;

namespace Api.Controllers
{
    [ApiVersion("1")]
    [Display(Name = "تنظیمات عملیاتی هر زیر سیستم")]
    public class SubSystemConfigurationController : CrudController<SubSystemConfigurationDto,
        SubSystemConfigurationResDto, SubSystemConfiguration>
    {
        public SubSystemConfigurationController(IRepository<SubSystemConfiguration> repository, IMapper mapper,
            IHashEntityValidator hashEntityValidator,
            IRepository<Role> roleRepository, RoleManager<Role> roleManager) : base(repository, mapper,
            hashEntityValidator, roleRepository, roleManager)
        {
        }

        protected override IQueryable<SubSystemConfiguration> setSearch(string? search,
            IQueryable<SubSystemConfiguration> query)
        {
            if (string.IsNullOrEmpty(search)) return query;
            query = query.Where(i => i.Title.Contains(search)
                                     || i.Value.Contains(search));
            return query;
        }
    }
}