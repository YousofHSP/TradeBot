﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Domain.Entities.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities;

[Display(Name = "نقش")]
public class Role : IdentityRole<int>, IBaseEntity<int>, ISoftDelete, IHashedEntity
{
    public string? Title { get; set; }
    public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? DeleteDate { get; set; } = null;
    [IgnoreDataMember]
    public List<UserGroup> UserGroups { get; set; }
    public string Hash { get; set; }
    public string SaltCode { get; set; }
}
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasMany(i => i.UserGroups)
            .WithMany(i => i.Roles);
    }
}