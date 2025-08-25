using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Shared.DTOs;

public class DepositUserDto
{
    
}

public class DepositUserResDto: BaseDto<DepositUserResDto, DepositUser>
{
    [Display(Name = "کاربر")]
    public string UserFullName { get; set; }
    [Display(Name = "مبلغ شروع")]
    public decimal StartAmount { get; set; }
    [Display(Name = "مبلغ پایان")]
    public decimal EndAmount { get; set; }

    protected override void CustomMappings(IMappingExpression<DepositUser, DepositUserResDto> mapping)
    {
        mapping.ForMember(i => i.UserFullName,
            s => s.MapFrom(m => m.User.Info != null ? $"{m.User.Info.FirstName} {m.User.Info.LastName}" : ""));
    }
}