using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities;

public class Coin: BaseEntity
{
    public string Currency { get; set; } = string.Empty; // unique currency code that never change
    public string Name { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public float ProfitLimit { get; set; }
    public float LoseLimit { get; set; }

    public List<Order> Orders { get; set; } = [];
}

public class CoinConfiguration : IEntityTypeConfiguration<Coin>
{
    public void Configure(EntityTypeBuilder<Coin> builder)
    {
        builder.HasIndex(i => i.Currency).IsUnique();
        builder
            .HasMany(c => c.Orders)
            .WithOne(o => o.Coin)
            .HasForeignKey(o => o.Currency)
            .HasPrincipalKey(c => c.Currency);
    }
}