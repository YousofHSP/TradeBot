using Domain.Entities;

namespace Shared.DTOs;

public class AuditDto : BaseDto<AuditDto, Audit>
{

    public string Ip { get; set; }
    public string CurrentLink { get; set; }
    public string ReferrerLink { get; set; }
    public string Protocol { get; set; }
    public string PhysicalPath { get; set; }
    public string Model { get; set; }
    public int UserId { get; set; }
    public string Method { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string Browser { get; set; }
    public string OperationSystem { get; set; }
}