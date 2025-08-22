using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Data.Contracts;
using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Message;
using Shared.DTOs;

namespace Service.Hubs
{
    public class AppHub : Hub
    {
        private readonly IRepository<Setting> _settingReposiroty;
        private readonly IRepository<ApiToken> _apiTokenReposiroty;
        private readonly RoleManager<Role> _roleManager;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ILogger<AppHub> _logger;

        public AppHub(IRepository<Setting> reposiroty, IMapper mapper, ILogger<AppHub> logger,
            INotificationService notificationService, IRepository<ApiToken> apiTokenReposiroty,
            RoleManager<Role> roleManager)
        {
            _settingReposiroty = reposiroty;
            _mapper = mapper;
            _logger = logger;
            _notificationService = notificationService;
            _apiTokenReposiroty = apiTokenReposiroty;
            _roleManager = roleManager;
        }

        public async Task SendNotifications(string code)
        {
            await _notificationService.SendNotifications(code, default);
        }

        public async Task SendPermissions(string code)
        {
            var apiToken = await _apiTokenReposiroty.TableNoTracking
                .Include(i => i.User)
                .ThenInclude(i => i.UserGroups)
                .ThenInclude(i => i.Roles)
                .FirstOrDefaultAsync(i => i.Code == code);
            if (apiToken is null)
                throw new AppException(Common.ApiResultStatusCode.UnAuthorized, "نشست معتبر نیست");
            var roles = apiToken.User.UserGroups.SelectMany(i => i.Roles).ToList();
            if (roles.Any(i => i.Id == 1))
            {
                await Clients.All.SendAsync("ReceivePermissions",
                    Permissions.All.Select(i => $"{i.Controller}.{i.Action}"));
                return;
            }

            var permissions = new List<Claim>();
            foreach (var role in roles)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                permissions.AddRange(claims);
            }

            await Clients.Caller.SendAsync("ReceivePermissions", permissions.Select(i => i.Value).ToArray());
        }

        public async Task SendMenu(string code)
        {
            var apiToken = await _apiTokenReposiroty.TableNoTracking
                .Include(i => i.User)
                .ThenInclude(i => i.UserGroups)
                .ThenInclude(i => i.Roles)
                .FirstOrDefaultAsync(i => i.Code == code);
            if (apiToken is null)
                throw new AppException(Common.ApiResultStatusCode.UnAuthorized, "نشست معتبر نیست");
            var roles = apiToken.User.UserGroups.SelectMany(i => i.Roles).ToList();
            var menus = new MenusDto();
            if (roles.Any(i => i.Id == 1))
            {
                await Clients.All.SendAsync("ReceiveMenu", code, menus.BuildMenuTree(menus.Items));
                return;
            }

            var permissions = new List<Claim>();
            foreach (var role in roles)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                permissions.AddRange(claims);
            }

            var menuItems = new List<MenuDto>();
            if (apiToken.UserId != 1)
                menuItems = menus.Items
                    .Where(i => i.ParentName == null || permissions.Any(p => i.Name == p.Value))
                    .ToList();
            else
                menuItems = menus.Items;
            var nestedMenus = menus.BuildMenuTree(menuItems);

            await Clients.All.SendAsync("ReceiveMenu", code, nestedMenus
                .Where(i => i.Name == "Dashboard" || (i.Children != null && i.Children.Any())).ToList());
        }

        public async Task SendSetting()
        {
            try
            {
                var list = await _settingReposiroty.TableNoTracking
                    .ProjectTo<SettingResDto>(_mapper.ConfigurationProvider).ToListAsync();
                await Clients.All.SendAsync("ReceiveSettings", list);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Faild To Send Setting");
            }
        }

        public async Task LogOut(string code)
        {
            try
            {
                await Clients.All.SendAsync("LogOut", code);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Faild To LogOut code: {code}");
            }
        }

        public override async Task OnConnectedAsync()
        {
            await SendSetting();
            await base.OnConnectedAsync();
        }
    }
}