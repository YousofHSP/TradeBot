﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities
{
    public class IpRule : BaseEntity, IHashedEntity
    {
        public string Ip { get; set; }
        public string? EndIp { get; set; }
        public string? Cidr { get; set; }
        public string Description { get; set; }
        public string Hash { get; set; }
        public string SaltCode { get; set; }

        public List<IpAccessType> IpAccessTypes { get; set; }
    }

    public class IpRuleConfiguration : IEntityTypeConfiguration<IpRule>
    {
        public void Configure(EntityTypeBuilder<IpRule> builder)
        {
            builder.HasMany(i => i.IpAccessTypes)
                .WithOne(i => i.IpRule)
                .HasForeignKey(i => i.IpRuleId);
            builder.HasOne(i => i.CreatorUser)
                .WithMany(u => u.CreatedIpRules)
                .HasForeignKey(i => i.CreatorUserId);
        }
    }
}
