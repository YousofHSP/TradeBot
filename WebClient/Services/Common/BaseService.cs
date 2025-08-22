using System.Net;
using Common;
using Common.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared;
using WebClient.Services.Components;
using Exception = System.Exception;

namespace WebClient.Services.Common;

public class BaseService : IBaseService
{
    private readonly HttpClient _httpClient;
    private readonly ToastService _toastService;
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager NavManager;
    
    public BaseService(HttpClient httpClient, ToastService toastService, IJSRuntime jsRuntime, NavigationManager navManager)
    {
        _httpClient = httpClient;
        _toastService = toastService;
        _jsRuntime = jsRuntime;
        NavManager = navManager;
    }

    public async Task<TResDto?> Get<TResDto>(string uri) where TResDto : class
    {
        try
        {

            await SetHeaders();
            var result = await _httpClient.GetAsync(uri);
            if (result.StatusCode is HttpStatusCode.InternalServerError)
                throw new Exception("خطا در دریافت اطلاعات");
            var response = await result.Content.ReadFromJsonAsync<ApiResult<TResDto>>();
            if (response is null)
                throw new Exception("خطا در تبدیل اطلاعات");
            if (!response.isSuccess)
                throw new Exception(response.message);
            return response.Data;
        }
        catch (Exception e)
        {
            _toastService.ShowError(e.Message);
            return null;
        }
    }
    public async Task<bool> Delete(string uri)
    {
        try
        {

            await SetHeaders();
            var result = await _httpClient.DeleteAsync(uri);
            if (result.StatusCode is HttpStatusCode.InternalServerError)
                throw new Exception("خطا در دریافت اطلاعات");
            var response = await result.Content.ReadFromJsonAsync<ApiResult>();
            if (response is null)
                throw new Exception("خطا در تبدیل اطلاعات");
            if (!response.isSuccess)
                throw new Exception(response.message);
            return true;
        }
        catch (Exception e)
        {
            _toastService.ShowError(e.Message);
            return false;
        }
    }

    public HttpClient GetHttpClient()
        => _httpClient;

    public async Task<TResDto?> Post<TDto, TResDto>(string uri, TDto dto) where TResDto : class, new()
    {
        try
        {
            await SetHeaders();
            var result = await _httpClient.PostAsJsonAsync(uri, dto);

            if (result.StatusCode is HttpStatusCode.InternalServerError)
                throw new Exception("خطا در دریافت اطلاعات");
            var response = await result.Content.ReadFromJsonAsync<ApiResult<TResDto>>();
            if (response is null)
                throw new Exception("خطا در تبدیل اطلاعات");
            if (response.statusCode is ApiResultStatusCode.UnAuthorized)
                throw new UnauthorizedAccessException(response.message);
            if (!response.isSuccess)
                throw new Exception(response.message);

            return response.Data;
        }
        catch (UnauthorizedAccessException e)
        {
            _toastService.ShowError(e.Message);
            var returnUrl = Uri.EscapeDataString(NavManager.Uri);
            NavManager.NavigateTo($"/Auth/Login?returnUrl={returnUrl}", true);
            return null;
        }
        catch (Exception e)
        {
            _toastService.ShowError(e.Message);
            return null;
        }
    }

    public async Task<TResDto?> Patch<TDto, TResDto>(string uri, TDto dto) where TResDto : class, new()
    {
        try
        {
            await SetHeaders();
            var result = await _httpClient.PatchAsJsonAsync(uri, dto);

            if (result.StatusCode is HttpStatusCode.InternalServerError)
                throw new Exception("خطا در دریافت اطلاعات");
            var response = await result.Content.ReadFromJsonAsync<ApiResult<TResDto>>();
            if (response is null)
                throw new Exception("خطا در تبدیل اطلاعات");
            if (response.statusCode is ApiResultStatusCode.UnAuthorized)
                throw new UnauthorizedAccessException(response.message);
            if (!response.isSuccess)
                throw new Exception(response.message);

            return response.Data;
        }
        catch (UnauthorizedAccessException e)
        {
            _toastService.ShowError(e.Message);
            var returnUrl = Uri.EscapeDataString(NavManager.Uri);
            NavManager.NavigateTo($"/Auth/Login?returnUrl={returnUrl}", true);
            return null;
        }
        catch (Exception e)
        {
            _toastService.ShowError(e.Message);
            return null;
        }
    }
    private async Task SetHeaders()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwt_token");
        GetHttpClient().DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
    }
}