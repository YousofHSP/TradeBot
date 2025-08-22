using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities
{
    public class Setting : IEntity<int>, IHashedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public int IsSecurity { get; set; }
        public SettingStatus Status { get; set; }

        public string? Description { get; set; }
        public string Hash { get; set; }
        public string SaltCode { get; set; }
    }

    public enum SettingStatus
    {
        [Display(Name = "غیرفعال")]
        Disable,
        [Display(Name = "فعال")]
        Enable
    }


}
