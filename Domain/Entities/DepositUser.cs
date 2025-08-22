using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities;

public class DepositUser: BaseEntity
{
    public int UserId { get; set; }
    public int DepositId { get; set; }
    public decimal StartAmount { get; set; }
    public decimal EndAmount { get; set; }

    #region Navigations

    public User User { get; set; } = new();
    public Deposit Deposit { get; set; } = new();

    #endregion
}


public class DepositUserConfiguration : IEntityTypeConfiguration<DepositUser>
{
    public void Configure(EntityTypeBuilder<DepositUser> builder)
    {
        builder
            .HasOne(du => du.User)
            .WithMany(u => u.DepositUsers)
            .HasForeignKey(du => du.UserId);

        builder
            .HasOne(du => du.Deposit)
            .WithMany(d => d.DepositUsers)
            .HasForeignKey(du => du.DepositId);
    }
}