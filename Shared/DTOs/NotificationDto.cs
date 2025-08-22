using AutoMapper;
using Common.Utilities;
using Domain.Entities;

namespace Shared.DTOs
{
    public class NotificationDto : BaseDto<NotificationDto, Notification>
    {
    }
    public class NotificationResDto : BaseDto<NotificationResDto, Notification>
    {
        public string Title { get; set; }
        public string CreateDate { get; set; }
        public string UserName { get; set; }
        public string Minutes { get; set; }
        public string SeenDate { get; set; }
        public NotificationStatus Status { get; set; }

        protected override void CustomMappings(IMappingExpression<Notification, NotificationResDto> mapping)
        {
            mapping.ForMember(
                d => d.CreateDate,
                s => s.MapFrom(m => m.CreateDate.ToShamsi(true)));
            mapping.ForMember(
                d => d.UserName,
                s => s.MapFrom(m => m.User.UserName));


            mapping.ForMember(
                d => d.SeenDate,
                s => s.MapFrom(m => m.SeenDate == null ? "" : m.SeenDate.Value.ToShamsi(true)));
            mapping.ForMember(
                d => d.Minutes,
                s => s.MapFrom(m => (DateTimeOffset.Now - m.CreateDate).TotalMinutes.ToString("00")));
        }
    }
}
