using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities;

public class Order: BaseEntity
{
    public long OrderId { get; set; }
    public string Currency { get; set; } = null!;
    public int? DepositId { get; set; }

    #region Navigations

    public Deposit? Deposit { get; set; }
    public Coin Coin { get; set; } = new();

    #endregion
}

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .HasOne(o => o.Deposit)
            .WithMany(d => d.Orders)
            .HasForeignKey(o => o.DepositId);
        builder.HasOne(o => o.Coin)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.Currency)
            .HasPrincipalKey(c => c.Currency);
    }
}