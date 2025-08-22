using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DTOs;

namespace Service.Hubs
{
    public class SettingHub : Hub
    {
        private readonly IRepository<Setting> _reposiroty;
        private readonly IMapper _mapper;
        private readonly ILogger<SettingHub> _logger;

        public SettingHub(IRepository<Setting> reposiroty, IMapper mapper)
        {
            _reposiroty = reposiroty;
            _mapper = mapper;
        }

        public async Task SendSetting()
        {

            try
            {

                var list = await _reposiroty.TableNoTracking
                    .ProjectTo<SettingResDto>(_mapper.ConfigurationProvider).ToListAsync();
                await Clients.All.SendAsync("ReceiveSettings", list);
            }
            catch (Exception e)
            {

                _logger.LogWarning("Faild To Send Setting");
            }
        }
        public override async Task OnConnectedAsync()
        {

            await SendSetting();
            await base.OnConnectedAsync();
        }
    }
}
