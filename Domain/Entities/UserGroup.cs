using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities;

public class UserGroup : BaseEntity, ISoftDelete
{
    public string Title { get; set; }
    public int SubSystemId { get; set; }

    public SubSystem SubSystem { get; set; }
    public List<Role> Roles { get; set; } = [];
    public List<User> Users { get; set; } = [];
    public DateTimeOffset? DeleteDate { get; set; }
}

public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.HasOne(i => i.SubSystem)
            .WithMany(i => i.UserGroups)
            .HasForeignKey(i => i.SubSystemId);
        builder.HasMany(i => i.Roles)
            .WithMany(i => i.UserGroups);
        
        builder.HasMany(i => i.Users)
            .WithMany(i => i.UserGroups);
        builder.HasOne(i => i.CreatorUser)
            .WithMany(i => i.CreatedUserGroups)
            .HasForeignKey(i => i.CreatorUserId);
    }
}