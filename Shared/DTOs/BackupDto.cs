﻿using AutoMapper;
using Common.Utilities;
using Domain.Entities;

namespace Shared.DTOs
{
    public class BackupDto : BaseDto<BackupDto, Backup>
    {
        public string UserName { get; set; }
        public string Date { get; set; }
        public string FileName { get; set; }

        protected override void CustomMappings(IMappingExpression<Backup, BackupDto> mapping)
        {
            mapping.ForMember(
                d => d.UserName,
                s => s.MapFrom(m => m.CreatorUser.UserName));

            mapping.ForMember(
                d => d.Date,
                s => s.MapFrom(m => m.CreateDate.ToShamsi(default)));
        }
    }
}
