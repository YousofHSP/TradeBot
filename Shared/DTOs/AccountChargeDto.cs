using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Shared.DTOs;

public class AccountChargeDto
{
    
        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")] 
        public int UserId { get; set; }
        
        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")] 
        public int Amount { get; set; }

        [Display(Name = "نوع")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")] 
        public TransactionType Type { get; set; }
        
}