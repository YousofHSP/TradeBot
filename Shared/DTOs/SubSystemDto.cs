using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;
using Shared.Validations;

namespace Shared.DTOs
{
    public class SubSystemDto : BaseDto<SubSystemDto, SubSystem>
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Display(Name = "شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [ExistsInDatabase<City>(nameof(City.Id), ErrorMessage = "{0} پیدا نشد")]
        public int CityId { get; set; }

        [Display(Name = "تماس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Tel { get; set; }

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "کدپستی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PostalCode { get; set; }

        [Display(Name = "کاربر مدیر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [ExistsInDatabase<User>(nameof(User.Id), ErrorMessage = "کاربر پیدا نشد")]
        public int AdminUserId { get; set; }
    }
    public class SubSystemResDto : BaseDto<SubSystemResDto, SubSystem>
    {

        public string Title { get; set; }
        public string CityTitle { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string AdminUserName { get; set; }
        public string AdminFullNmae { get; set; }

        protected override void CustomMappings(IMappingExpression<SubSystem, SubSystemResDto> mapping)
        {
            mapping.ForMember(
                d => d.AdminUserName,
                s => s.MapFrom(m => m.AdminUser.UserName)
                );
            mapping.ForMember(
                d => d.AdminFullNmae,
                s => s.MapFrom(m => m.AdminUser.Info.FirstName + " " + m.AdminUser.Info.LastName)
                );
            base.CustomMappings(mapping);
        }
    }
}
