using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.DTOs;
using WebClient.Services.Common;
using WebClient.Services.Components;
using Timer = System.Timers.Timer;

namespace WebClient.Components.Pages.Coin;

public partial class CoinIndex : ComponentBase
{
    private List<CoinResDto> _list = [];
    private List<CoinResDto> _notStoredlist = [];
    private CoinDto _data = new();
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
        var result = await BaseService.Post<IndexDto, IndexResDto<CoinResDto>>("v1/Coin/Index", _indexDto);
        var notStoreds = await BaseService.Post<IndexDto, IndexResDto<CoinResDto>>("v1/Coin/GetNotStored", _indexDto);
        if (result is not null)
        {
            _list = result.Data;
            _indexDto.Page = result.Page;
            _indexDto.Limit = result.Limit;
            _total = result.Total;
        }

        if (notStoreds is not null)
            _notStoredlist = notStoreds.Data;

        if (result is not null && notStoreds is not null)
        {
            _isLoading = false;
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
            var deleteResult = await BaseService.Delete($"v1/Coin/{_data.Id}");
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

    public async Task DoneForm()
    {
        ToastService.ShowSuccess("اطلاعات  با موفقیت ثبت شد");
        await Js.InvokeVoidAsync("closeModal", "dataModal");
        await GetData();
    }

    private async Task ShowDeleteWarning(int id)
    {
        await Js.InvokeVoidAsync("openModal", "deleteModal");
        _data = new() { Id = id };
    }
}
