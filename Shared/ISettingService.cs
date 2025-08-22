﻿using Shared.DTOs;

namespace Shared
{
    public interface ISettingService
    {
        Task<T> GetValueAsync<T>(SettingKey key);
        T GetValue<T>(SettingKey key);
        Task NotifyClientsAsync();

    }
}
