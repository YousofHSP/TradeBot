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
    public class SubSystemDataInitializer(IRepository<SubSystem> Repository) : IDataInitializer
    {
        public async Task InitializerData()
        {
            var model = new SubSystem
            {
                Address = "address",
                AdminUserId = 1,
                CityId = 1,
                CreatorUserId = 1,
                Status = SubSystemStatus.Enable,
                PostalCode = "123456",
                Tel = "1234567890",
                Title = "subsystem"
            };

            var any = await Repository.TableNoTracking.AnyAsync();
            if (!any)
                await Repository.AddAsync(model, default);
        }
    }
}
