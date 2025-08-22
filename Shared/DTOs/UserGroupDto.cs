using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Shared.DTOs;

public class UserGroupDto : BaseDto<UserGroupDto, UserGroup>
{
    [Display(Name = "عنوان")]public string Title { get; set; }
    [Display(Name = "نقش ها")]public List<int> RoleIds { get; set; }
}

public class UserGroupResDto : BaseDto<UserGroupResDto, UserGroup>
{
    [Display(Name = "عنوان")] public string Title { get; set; }

    [Display(Name = "نقش ها")] public string RoleTitles { get; set; }
     public List<int> RoleIds{ get; set; }

    protected override void CustomMappings(IMappingExpression<UserGroup, UserGroupResDto> mapping)
    {
        mapping.ForMember(d => d.RoleIds,
            s => s.MapFrom(m => m.Roles.Select(i => i.Id)));
        mapping.ForMember(
            d => d.RoleTitles,
            s => s.MapFrom(m => string.Join(", ", m.Roles.Select(i => i.Title))));
    }
}