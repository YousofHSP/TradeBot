using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities;

public class Deposit: BaseEntity
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public decimal StartAmount { get; set; }
    public decimal? EndAmount { get; set; }

    #region Navigatins

    public List<DepositUser> DepositUsers { get; set; } = [];
    public List<Order> Orders { get; set; } = [];

    #endregion

}

public class DepositConfiguration : IEntityTypeConfiguration<Deposit>
{
    public void Configure(EntityTypeBuilder<Deposit> builder)
    {
        builder
            .HasMany(d => d.DepositUsers)
            .WithOne(du => du.Deposit)
            .HasForeignKey(du => du.DepositId);

        builder
            .HasMany(d => d.Orders)
            .WithOne(o => o.Deposit)
            .HasForeignKey(o => o.DepositId);
    }
}