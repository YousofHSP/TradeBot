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
    public class ApiToken : IBaseEntity<int>
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public string Code { get; set; }
        public int UserId { get; set; }
        public int? SubSystemId { get; set; }
        public ApiTokenStatus Status { get; set; }
        public DateTimeOffset LastUsedDate { get; set; }

        [IgnoreDataMember] public User User { get; set; }

        [IgnoreDataMember] public SubSystem? SubSystem { get; set; } 
    }

    public class ApiTokenConfiguration : IEntityTypeConfiguration<ApiToken>
    {
        public void Configure(EntityTypeBuilder<ApiToken> builder)
        {
            builder.HasOne(a => a.User)
                .WithMany(u => u.ApiTokens)
                .HasForeignKey(a => a.UserId);

            builder.HasOne(a => a.SubSystem)
                .WithMany(u => u.ApiTokens)
                .HasForeignKey(a => a.SubSystemId);

        }
    }

    public enum ApiTokenStatus
    {
        [Display(Name = "غیرفعال")]
        Disable,
        [Display(Name = "فعال")]
        Enable
    }
}
