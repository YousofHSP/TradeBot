using System.ComponentModel.DataAnnotations;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities;

public class Transaction: BaseEntity
{
    public int UserId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }

    #region Navigations

    public User User { get; set; }

    #endregion
}

public enum TransactionType
{
    [Display(Name = "برداشت از حساب")]
    Decrease,
    [Display(Name = "واریز به حساب")]
    Increase,
    [Display(Name = "شروع سرمایه گذاری")]
    Deposit,
    [Display(Name = "پایان سرمایه گذاری")]
    EndDeposit
}

public class TransactionConfigurations : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder
            .HasOne(t => t.User)
            .WithMany(user => user.Transactions)
            .HasForeignKey(t => t.UserId)
            .IsRequired();
    }
}