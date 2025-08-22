using AutoMapper;
using Common.Utilities;
using Domain.Entities;

namespace Shared.DTOs
{

    public class ArchiveLogResDto : BaseDto<ArchiveLogResDto, ArchiveLog>
    {
        public string ArchiveFileName { get; set; }
        public string ArchivedUntilDate { get; set; }
        public string ArchivedAt { get; set; }
        public int LogCount { get; set; }
        public int AuditCount { get; set; }

        protected override void CustomMappings(IMappingExpression<ArchiveLog, ArchiveLogResDto> mapping)
        {
            mapping.ForMember(
                d => d.ArchivedUntilDate,
                s => s.MapFrom(m => m.ArchivedUntilDate.ToShamsi(default)));
            mapping.ForMember(
                d => d.ArchivedAt,
                s => s.MapFrom(m => m.ArchivedAt.ToShamsi(default)));
        }
    }
    public class ArchiveRequestDto
    {
        public string UntilDate { get; set; }
    }
}
