using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;
using Shared.Validations;

namespace Shared.DTOs
{
    public class SubSystemConfigurationDto : BaseDto<SubSystemConfigurationDto, SubSystemConfiguration>
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "تعداد کاراکتر {0} نباید بیشتر از {1} باشد")]
        public string Title { get; set; }

        [Display(Name = "مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Value { get; set; }

        [Display(Name = "زیرسیستم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [ExistsInDatabase<SubSystem>(nameof(SubSystem.Id), ErrorMessage = "زیرسیستم پیدا نشد")]
        public int SubSystemId { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public SubSystemConfigurationStatus Status{ get; set; }
    }
    public class SubSystemConfigurationResDto : BaseDto<SubSystemConfigurationResDto, SubSystemConfiguration>
    {

        public string Title { get; set; }
        public string Value { get; set; }
        public int SubSystemId { get; set; }
        public string SubSystemTitle { get; set; }
        public string CreatorUserName { get; set; }
        public SubSystemConfigurationStatus Status { get; set; }

        protected override void CustomMappings(IMappingExpression<SubSystemConfiguration, SubSystemConfigurationResDto> mapping)
        {
            mapping.ForMember(
                d => d.CreatorUserName,
                s => s.MapFrom(m => m.CreatorUser.UserName));
        }
    }
}
