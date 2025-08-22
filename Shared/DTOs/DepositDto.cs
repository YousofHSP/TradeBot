using AutoMapper;
using Domain.Entities;

namespace Shared.DTOs;

public class DepositDto: BaseDto<DepositDto, Deposit>
{
    
}

public class DepositResDto: BaseDto<DepositResDto, Deposit>
{

    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public decimal StartAmount { get; set; }
    public decimal EndAmount { get; set; }

    protected override void CustomMappings(IMappingExpression<Deposit, DepositResDto> mapping)
    {
        mapping
            .ForMember(
                i => i.StartDate,
                s => s.MapFrom(source => source.StartDate.ToString())
            );
        mapping
            .ForMember(
                i => i.EndDate,
                s => s.MapFrom(source => source.EndDate.ToString())
            );
        base.CustomMappings(mapping);
    }
}