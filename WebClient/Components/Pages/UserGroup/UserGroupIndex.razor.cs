using Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.DTOs;
using WebClient.Services.Common;
using WebClient.Services.Components;
using Timer = System.Timers.Timer;

namespace WebClient.Components.Pages.UserGroup;

public partial class UserGroupIndex : ComponentBase
{
    private List<UserGroupResDto> _list = [];
    private UserGroupDto _data = new();
    private List<SelectDto> _roles = [];
    private readonly IndexDto _indexDto = new();
    private Timer? _debounceTimer;
    private int _total;
    private string _modalTitle = "";
    private bool _modalIsBusy;
    private bool _isBusy;
    private bool _isLoading = true;

    [Inject] public IJSRuntime Js { get; set; } = null!;
    [Inject] public IBaseService BaseService { get; set; } = null!;
    [Inject] public ToastService ToastService { get; set; } = null!;

    private async Task GetData()
    {
        var result = await BaseService.Post<IndexDto, IndexResDto<UserGroupResDto>>("v1/UserGroup/Index", _indexDto);
        if (result is not null)
        {
            _list = result.Data;
            _indexDto.Page = result.Page;
            _indexDto.Limit = result.Limit;
            _total = result.Total;
            _isLoading = false;
            var roles =
                await BaseService.Get<List<SelectDto>>("v1/Role/GetSelectList");

            if (roles is not null)
            {
                _roles = roles;
            }

            StateHasChanged();
        }
    }

    private void OnSearchChanged(ChangeEventArgs e)
    {
        _indexDto.Search = e.Value?.ToString() ?? "";

        // اگر تایمر قبلی وجود داشت ریست بشه
        _debounceTimer?.Stop();
        _debounceTimer?.Dispose();

        // یه تایمر جدید با 1 ثانیه تأخیر
        _debounceTimer = new Timer(500) { AutoReset = false };
        _debounceTimer.Elapsed += async (_, _) =>
        {
            // صدا زدن سرور
            await InvokeAsync(GetData);
        };
        _debounceTimer.Start();
    }

    private async Task PageChanged((int Page, int Limit) args)
    {
        _indexDto.Page = args.Page;
        _indexDto.Limit = args.Limit;
        await GetData();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                await GetData();
            }
        }
        catch

        {
            Console.WriteLine("error");
        }
    }

    private async Task Delete()
    {
        await Js.InvokeVoidAsync("closeModal", "deleteModal");
        _isLoading = true;
        try
        {
            var deleteResult = await BaseService.Delete($"v1/UserGroup/{_data.Id}");
            if (deleteResult)
            {
                ToastService.ShowSuccess(" حذف شد");
                await GetData();
            }
        }
        finally
        {
            _data = new();
            _isLoading = false;
        }
    }

    private async Task HandleValidSubmit()
    {
        try
        {
            _isBusy = true;
            UserGroupResDto? result;

            if (_modalTitle == "ویرایش")
            {
                result = await BaseService.Patch<UserGroupDto, UserGroupResDto>("v1/UserGroup/Update", _data);
            }
            else
            {
                result = await BaseService.Post<UserGroupDto, UserGroupResDto>("v1/UserGroup/Create", _data);
            }

            if (result is not null)
            {
                ToastService.ShowSuccess("اطلاعات  با موفقیت ثبت شد");
                await Js.InvokeVoidAsync("closeModal", "dataModal");
                await GetData();
            }
        }
        finally
        {
            _isBusy = false;
        }
    }

    private async Task ShowCreateModal()
    {
        _modalTitle = "ایجاد";
        _data = new();
        await Js.InvokeVoidAsync("openModal", "dataModal");
    }

    private async Task ShowEditModal(int id)
    {
        await Js.InvokeVoidAsync("openModal", "dataModal");
        _modalTitle = "ویرایش";
        _modalIsBusy = true;
        try
        {
            var result = await BaseService.Get<UserGroupResDto>($"v1/UserGroup/{id}");
            if (result is not null)
            {
                _data.Title = result.Title;
                _data.Id = result.Id;
                _data.RoleIds = result.RoleIds;
                _modalIsBusy = false;
                StateHasChanged();
            }
        }
        finally
        {
            _modalIsBusy = false;
        }
    }

    private async Task ShowDeleteWarning(int id)
    {
        await Js.InvokeVoidAsync("openModal", "deleteModal");
        _data = new UserGroupDto { Id = id };
    }
}