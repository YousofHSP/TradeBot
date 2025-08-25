using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Common.Utilities;
using Domain.Entities;

namespace Shared.DTOs;

public class DepositDto: BaseDto<DepositDto, Deposit>
{
    
}

public class DepositResDto: BaseDto<DepositResDto, Deposit>
{

    [Display(Name = "تاریخ شروع")]
    public DateTimeOffset StartDate { get; set; }
    [Display(Name = "تاریخ پایان")]
    public DateTimeOffset? EndDate { get; set; }
    
    [Display(Name = "مبلغ شروع")]
    public decimal StartAmount { get; set; }
    [Display(Name = "مبلغ پایان")]
    public decimal EndAmount { get; set; }

}