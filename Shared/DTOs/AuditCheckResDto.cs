using AutoMapper;
using Common.Utilities;
using Domain.Entities;

namespace Shared.DTOs
{
    public class AuditCheckResDto : BaseDto<AuditCheckResDto, AuditCheck>
    {
        public string CreateDate { get; set; }
        public int TablesCheckCount { get; set; }
        public string UserName { get; set; }
        public int EntitiesCheckCount { get; set; }
        public int RestoredEntitiesCount { get; set; }

        protected override void CustomMappings(IMappingExpression<AuditCheck, AuditCheckResDto> mapping)
        {
            mapping.ForMember(
                d => d.CreateDate,
                s => s.MapFrom(m => m.CreateDate.ToShamsi(true)));
            mapping.ForMember(
                d => d.UserName,
                s => s.MapFrom(m => m.CreatorUser.UserName));
            mapping.ForMember(
                d => d.EntitiesCheckCount,
                s => s.MapFrom(m => m.AuditCheckDetails.Count));
            mapping.ForMember(
                d => d.RestoredEntitiesCount,
                s => s.MapFrom(m => m.AuditCheckDetails.Count(i => i.Status == AuditCheckDetailStatus.Invalid)));


        }
    }

    public class AuditCheckDetailResDto : BaseDto<AuditCheckDetailResDto, AuditCheckDetail>
    {
        public string Model { get; set; }
        public int ModelId { get; set; }
        public AuditCheckDetailStatus Status{ get; set; }
        public string AuditCreateDate { get; set; }

        protected override void CustomMappings(IMappingExpression<AuditCheckDetail, AuditCheckDetailResDto> mapping)
        {
            mapping.ForMember(
                d => d.AuditCreateDate,
                s => s.MapFrom(m => m.AuditCreateDate != null ? m.AuditCreateDate.Value.ToShamsi(true) : "-"));
        }

    }

}
