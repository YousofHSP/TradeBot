using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Service.DataInitializer
{
    public class SubSystemConfigurationDataInitializer(IRepository<SubSystemConfiguration> Repository) : IDataInitializer
    {
        public async Task InitializerData()
        {
            var list = new List<SubSystemConfiguration>
            {
                new () { CreatorUserId = 1, SubSystemId =  1, Title = "سال مرجع", Value ="1404", Status = SubSystemConfigurationStatus.Enable},
            };

            var any = await Repository.TableNoTracking.AnyAsync();
            if (!any)
            {
                await Repository.AddRangeAsync(list, default);
            }
        }
    }
}
