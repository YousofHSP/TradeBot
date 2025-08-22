using AutoMapper;
using Common.Utilities;
using Domain.Entities;

namespace Shared.DTOs
{
    public class ApiTokenResDto : BaseDto<ApiTokenResDto, ApiToken>
    {
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public string CreateDate { get; set; }
        public string Code { get; set; }
        public ApiTokenStatus Enable { get; set; }
        public string UserName { get; set; }
        public string LastUsedDate { get; set; }

        protected override void CustomMappings(IMappingExpression<ApiToken, ApiTokenResDto> mapping)
        {
            mapping.ForMember(
                d => d.CreateDate,
                s => s.MapFrom(m => m.CreateDate.ToShamsi(true)));
            mapping.ForMember(
                d => d.LastUsedDate,
                s => s.MapFrom(m => m.LastUsedDate.ToShamsi(true)));
            
            mapping.ForMember(
                d => d.UserName,
                s => s.MapFrom(m => m.User.UserName));
        }
    }
}
