using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Common.Utilities;
using Domain.Entities;

namespace Shared.DTOs
{
    public class SmsLogResDto : BaseDto<SmsLogResDto, SmsLog>
    {
        public string Text { get; set; }
        public string CreateDate { get; set; }
        public string CreatorUserName { get; set; }
        public string Mobile { get; set; }

        protected override void CustomMappings(IMappingExpression<SmsLog, SmsLogResDto> mapping)
        {
            mapping.ForMember(
                d => d.CreatorUserName,
                s => s.MapFrom(m => m.CreatorUser.UserName));
            mapping.ForMember(
                d => d.CreateDate,
                s => s.MapFrom(m => m.CreateDate.ToShamsi(default)));
        }

    }
    public class ReSendDto
    {
        [Required]
        public int Id { get; set; }
    }


}
