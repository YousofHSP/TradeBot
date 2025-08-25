using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.DTOs;
using WebClient.Services.Common;
using WebClient.Services.Components;

namespace WebClient.Components.Pages.Deposit;

public partial class DepositIndex : ComponentBase
{
    private List<DepositResDto> _list = [];
    private readonly IndexDto _indexDto = new();
    private List<DepositUserResDto> _users = [];
    private bool _isLoading = true;
    private bool _modalIsBusy;
    private string _modalTitle = "";
    private int _total;

    [Inject] private IBaseService BaseService { get; set; } = null!;
    [Inject] public IJSRuntime Js { get; set; } = null!;
    [Inject] public ToastService ToastService { get; set; } = null!;

    private async Task GetData()
    {
        var result = await BaseService.Post<IndexDto, IndexResDto<DepositResDto>>("v1/Deposit/Index", _indexDto);
        if (result is not null)
        {
            _list = result.Data;
            _indexDto.Page = result.Page;
            _indexDto.Limit = result.Limit;
            _total = result.Total;
            _isLoading = false;
            StateHasChanged();
        }
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

    private async Task ShowUsers(int id)
    {
        await Js.InvokeVoidAsync("openModal", "dataModal");
        _modalTitle = "کاربران";
        _modalIsBusy = true;
        try
        {
            var result = await BaseService.Get<List<DepositUserResDto>>($"v1/Deposit/GetDepositUsers?depositId={id}");
            if (result is not null)
            {
                _users = result;
                _modalIsBusy = false;
                StateHasChanged();
            }
        }
        finally
        {
            _modalIsBusy = false;
        }
    }
}