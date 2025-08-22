using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities
{
    public class SubSystemConfiguration : BaseEntity, IHashedEntity
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public int SubSystemId { get; set; }
        public string Hash { get; set; }
        public string SaltCode { get; set; }
        public SubSystemConfigurationStatus Status { get; set; }

        #region Rels
        public SubSystem SubSystem { get; set; }
        #endregion

    }

    public class SubSystemTypeConfguration : IEntityTypeConfiguration<SubSystemConfiguration>
    {
        public void Configure(EntityTypeBuilder<SubSystemConfiguration> builder)
        {
            builder.Property(i => i.Title).IsRequired().HasMaxLength(500);

            builder.HasOne(i => i.SubSystem)
                .WithMany(i => i.SubSystemConfigurations)
                .HasForeignKey(i => i.SubSystemId);
            builder.HasOne(i => i.CreatorUser)
                .WithMany(i => i.CreatedSubSystemConfigurations)
                .HasForeignKey(i => i.SubSystemId);
        }
    }

    public enum SubSystemConfigurationStatus 
    {
        [Display(Name = "غیرفعال")]
        Disable,
        [Display(Name = "فعال")]
        Enable
    }
}
