using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Service.Hubs;
using Shared;
using Shared.DTOs;

namespace Domain.Model
{
    public class SettingService : ISettingService
    {
        private readonly IRepository<Setting> _repository;
        private readonly IHubContext<AppHub> _hubContext;
        private readonly IMapper _mapper;

        public SettingService(IRepository<Setting> repository, IHubContext<AppHub> hubContext, IMapper mapper)
        {
            _repository = repository;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        public async Task<T> GetValueAsync<T>(SettingKey key)
        {
            var item = await _repository.TableNoTracking.FirstOrDefaultAsync(i => i.Id == (int)key);
            if (item == null || string.IsNullOrEmpty(item.Value))
                return default;

            return Convert<T>(item.Value);


        }
        public T GetValue<T>(SettingKey key)
        {
            var item = _repository.TableNoTracking.FirstOrDefault(i => i.Id == (int)key);
            if (item == null || string.IsNullOrEmpty(item.Value))
                return default;

            return Convert<T>(item.Value);


        }
        private T Convert<T>(string value)
        {
            var type = typeof(T);
            if (type == typeof(int))
            {
                return (T)(object)int.Parse(value);
            }

            if (type == typeof(string[]))
            {
                return (T)(object)value.Split(',');
            }

            if (type == typeof(List<string>))
            {
                return (T)(object)value.Split(',').Select(x => x.Trim()).ToList();
            }
            if (type == typeof(int[]))
            {
                return (T)(object)value.Split(',').Select(int.Parse).ToArray();
            }

            if (type == typeof(List<int>))
            {
                return (T)(object)value.Split(',').Select(int.Parse).ToList();
            }

            return (T)(object)value;

        }

        public async Task NotifyClientsAsync()
        {
            var settings = await _repository.TableNoTracking
                .ProjectTo<SettingDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveSettings", settings);
        }
    }

}
