using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Data.Contracts;
using Domain.Entities;
using Domain.Entities.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Service.Hubs;
using Shared.DTOs;

namespace Service.Message
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> _repository;
        private readonly IRepository<ApiToken> _apiTokenRepository;
        private IHubContext<AppHub> _appHubContext;
        private IMapper _mapper;

        public NotificationService(IRepository<Notification> repository, IHubContext<AppHub> appHubContext, IMapper mapper, IRepository<ApiToken> apiTokenRepository)
        {
            _repository = repository;
            _appHubContext = appHubContext;
            _mapper = mapper;
            _apiTokenRepository = apiTokenRepository;
        }

        public async Task SendNotification(int userId, string title, int creatorUserId, CancellationToken ct)
        {
            var model = new Notification
            {
                UserId = userId,
                CreatorUserId = creatorUserId,
                Title = title,
                Status = NotificationStatus.Unseen
            };
            await _repository.AddAsync(model, ct);
            var count  = await _repository.TableNoTracking.CountAsync(i => i.UserId == userId && i.Status == NotificationStatus.Unseen, ct);
            var dto = NotificationResDto.FromEntity(model, _mapper);
            var apiTokens = await _apiTokenRepository.TableNoTracking
                .Where(i => i.UserId == userId && i.Status == ApiTokenStatus.Enable)
                .ToListAsync(ct);

            await _appHubContext.Clients.All.SendAsync("ReceiveNotification", dto, count, apiTokens.Select(i => i.Code), ct);

        }
        public async Task SendNotifications(string apiTokenCode, CancellationToken ct)
        {
            var apiToken = await _apiTokenRepository.TableNoTracking
                .FirstOrDefaultAsync(i => i.Code == apiTokenCode && i.Status == ApiTokenStatus.Enable, ct);
            if (apiToken is null)
                throw new AppException(Common.ApiResultStatusCode.UnAuthorized, "سشن معتبر نیست");
            var list = await _repository.TableNoTracking
                .Where(i => i.UserId == apiToken.UserId)
                .Where(i => i.Status == NotificationStatus.Unseen)
                .ProjectTo<NotificationResDto>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);

            await _appHubContext.Clients.All.SendAsync("ReceiveNotifications", list, list.Count, apiTokenCode, ct);
        }
    }
}
