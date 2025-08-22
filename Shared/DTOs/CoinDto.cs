using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Shared.DTOs;

public class CoinDto: BaseDto<CoinDto, Coin>
{
    [Required] public string Currency { get; set; } = null!;
    [Required] public float ProfitLimit { get; set; }
    [Required] public float LoseLimit { get; set; }
}

public class CoinResDto : BaseDto<CoinResDto, Coin>
{
    public string Currency { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public float ProfitLimit { get; set; }
    public float LoseLimit { get; set; }
    public decimal? Price { get; set; }
}

public class BuyCoinDto: IValidatableObject
{
    
    [Display(Name = "ارز")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string Currency { get; set; }
    
    [Display(Name = "مقدار")]
    public decimal Amount { get; set; }
    
    [Display(Name = "مبلغ")]
    public decimal Price { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IsInvalid(Amount) && IsInvalid(Price))
        {
            yield return new ValidationResult(
                "حداقل یکی از فیلدهای مقدار یا مبلغ باید مقدار داشته باشد و مقدار صفر معتبر نیست.",
                [nameof(Amount), nameof(Price)]);
        }

        yield break;

        bool IsInvalid(decimal? value) => value is null or 0;
    }
}