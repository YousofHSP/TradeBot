using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities
{
    public class SubSystem : BaseEntity, IHashedEntity, ISoftDelete
    {
        public string Title { get; set; }
        public int CityId { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public int AdminUserId { get; set; }
        public string Hash { get; set; }
        public string SaltCode { get; set; }
        public SubSystemStatus Status { get; set; }

        public DateTimeOffset? DeleteDate { get; set; }
        #region Rels

        [IgnoreDataMember]
        public User AdminUser { get; set; }
        [IgnoreDataMember]
        public List<UserGroup> UserGroups{ get; set; }

        [IgnoreDataMember]
        public City City { get; set; }
        [IgnoreDataMember]
        public List<SubSystemConfiguration> SubSystemConfigurations { get; set; }

        [IgnoreDataMember]
        public List<ApiToken> ApiTokens { get; set; }
        #endregion
    }

    public class SubSystemConfig: IEntityTypeConfiguration<SubSystem>
    {
        public void Configure(EntityTypeBuilder<SubSystem> builder)
        {
            builder.HasMany(i => i.UserGroups)
                .WithOne(i => i.SubSystem)
                .HasForeignKey(i => i.SubSystemId);
            builder.HasOne(s => s.CreatorUser)
                .WithMany(u => u.CreatedSubSystems)
                .HasForeignKey(s => s.CreatorUserId);
            builder.HasOne(s => s.City)
                .WithMany(c => c.SubSystems)
                .HasForeignKey(s => s.CityId);
            builder.HasOne(i => i.AdminUser)
                .WithMany(i => i.SubSystems)
                .HasForeignKey(i => i.AdminUserId);
            builder.HasMany(i => i.SubSystemConfigurations)
                .WithOne(i => i.SubSystem)
                .HasForeignKey(i => i.SubSystemId);
            builder.HasMany(i => i.ApiTokens)
                .WithOne(i => i.SubSystem)
                .HasForeignKey(i => i.SubSystemId);
        }
    }

    public enum SubSystemStatus
    {
        [Display(Name = "غیرفعال")]
        Disable,
        [Display(Name = "فعال")]
        Enable
    }
}
