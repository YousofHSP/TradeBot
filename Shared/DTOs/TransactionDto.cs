using AutoMapper;
using Common.Utilities;
using Domain.Entities;

namespace Shared.DTOs;

public class TransactionDto : BaseDto<TransactionDto, Transaction>
{
    public string UserFullName { get; set; }
    public decimal Amount { get; set; }
    public string UserPhoneNumber { get; set; }
    public string Type { get; set; }
    public string CreatedAt { get; set; }

    protected override void CustomMappings(IMappingExpression<Transaction, TransactionDto> mapping)
    {
        mapping
            .ForMember(
                i => i.CreatedAt,
                s => s.MapFrom(source => source.CreateDate.ToShamsi(false)));
        base.CustomMappings(mapping);
    }
}