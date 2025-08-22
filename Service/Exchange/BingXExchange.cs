using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients;
using Domain.Entities;
using Shared.DTOs;

namespace Service.Exchange;

public class BingXExchange
     : IExchange
{
    private readonly IBingXRestClient client;

    public BingXExchange(IBingXRestClient c)
    {
        client = c;
        // client = new BingXRestClient(options =>
        // {
        //     options.ApiCredentials = new ApiCredentials(
        //         "XUnyzKy7ABQyrOsYxDZxOX3CL4AUBJTu4MYNW3n5ljlveHHsriftHs40M9I3qkbvF9EKsxen0c2PiZAsoDQ",
        //         "aMlOt6rESXxSG0KqhgAlDTjdJdnMMelfqEwSxUT1bBQlxCq4CfTgWpo82Bnwit5U7bePB7BT6SRoeow6OQ");
        // });

    }
    public async Task<List<CoinResDto>> GetCoins(string? currency, CancellationToken ct)
    {
        var symbols = await client.SpotApi.ExchangeData.GetSymbolsAsync(currency, ct);
        if (!symbols.Success)
            throw new Exception("Failed to get symbols");
        var coins = new List<CoinResDto>();
        foreach (var symbol in symbols.Data)
            coins.Add(new CoinResDto
            {
                Currency = symbol.Name,
                FullName = symbol.Name,
                Name = symbol.Name,
            });

        return coins;
    }

    public async Task<CoinResDto?> GetCoin(string currency, CancellationToken ct)
    {
        var symbols = await client.SpotApi.ExchangeData.GetSymbolsAsync(currency, ct);
        if (!symbols.Success)
            throw new Exception("Failed to get symbols");
        var symbol = symbols.Data.First();
        var coin = new CoinResDto{
                Currency = symbol.Name,
                FullName = symbol.Name,
                Name = symbol.Name,
            };

        return coin;
    }

    public async Task<long> PlaceOrder(Coin coin, int leverage, CancellationToken ct)
    {
        var takeProfitPercent = (decimal)coin.ProfitLimit;
        var stopLossPercent = (decimal)coin.LoseLimit;
        // 1. تنظیم لوریج
        var leverageResult = await client.PerpetualFuturesApi.Account.SetLeverageAsync(
            symbol: coin.Currency,
            leverage: leverage,
            side: PositionSide.Both,
            ct: ct);
        
        if (!leverageResult.Success)
            throw new Exception($"Failed to set leverage: {leverageResult.Error?.Message}");

        // 2. گرفتن قیمت لحظه‌ای
        var priceResult = await client.PerpetualFuturesApi.ExchangeData.GetLastTradePriceAsync(coin.Currency, ct);
        if (!priceResult.Success)
            throw new Exception($"Failed to fetch price: {priceResult.Error?.Message}");

        var entryPrice = priceResult.Data.Price;

        // 3. گرفتن موجودی USDT
        var usdtBalance = await GetBalance(cancellationToken:ct);
        if (usdtBalance <= 0)
            throw new Exception("Insufficient USDT balance.");

        // 4. محاسبه حجم سفارش
        var usdtToSpend = usdtBalance * 0.30m;
        // var quantity = usdtToSpend / entryPrice;
        var quantity = 200;

        // 5. زدن سفارش مارکت خرید
        var orderResult = await client.PerpetualFuturesApi.Trading.PlaceOrderAsync(
            symbol: coin.Currency,
            side: OrderSide.Buy,
            type: FuturesOrderType.Market,
            positionSide: PositionSide.Both,
            quantity: quantity,
            
            ct: ct);
        //
        // if (!orderResult.Success)
        //     throw new Exception($"Order placement failed: {orderResult.Error?.Message}");
        //
        // var orderId = orderResult.Data.OrderId;

        var orderId = 1;
        // 6. محاسبه قیمت‌های تارگت و استاپ
        var takeProfitPrice = entryPrice * (1 + takeProfitPercent / 100);
        var stopLossPrice = entryPrice * (1 - stopLossPercent / 100);

        // 7. ثبت سفارش Take Profit (Limit Sell)
        var tpOrderResult = await client.PerpetualFuturesApi.Trading.PlaceOrderAsync(
            symbol: coin.Currency,
            side: OrderSide.Sell,
            type: FuturesOrderType.TakeProfitMarket,
            positionSide: PositionSide.Both,
            quantity: quantity,
            triggerPrice: Math.Round(takeProfitPrice, 4, MidpointRounding.AwayFromZero),
            ct: ct);
        
        if (!tpOrderResult.Success)
            throw new Exception($"Failed to place Take Profit order: {tpOrderResult.Error?.Message}");

        // 8. ثبت سفارش Stop Loss (Market Sell)
        var slOrderResult = await client.PerpetualFuturesApi.Trading.PlaceOrderAsync(
            symbol: coin.Currency,
            side: OrderSide.Sell,
            type: FuturesOrderType.StopMarket,
            positionSide: PositionSide.Both,
            quantity: quantity,
            triggerPrice: stopLossPrice,
            ct: ct);

        if (!slOrderResult.Success)
            throw new Exception($"Failed to place Stop Loss order: {slOrderResult.Error?.Message}");

        return orderId;
    }

    public Task<decimal> GetCoinPrice(Coin coin, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<long> CancelOrder(string currency, long? orderId)
    {
        var result = await client.PerpetualFuturesApi.Trading.CancelOrderAsync(currency, orderId);
        return result.Data.OrderId;
    }

    public async Task<decimal> GetBalance(string currency = "USDT", CancellationToken cancellationToken = default)
    {
        var result = await client.PerpetualFuturesApi.Account.GetBalancesAsync(cancellationToken);
        if (!result.Success)
            throw new Exception("Failed to get balance");

        decimal amount = 0m;
        foreach (var item in result.Data)
        {
            Console.WriteLine($"asset: {item.Asset} => free: {item.AvailableMargin}");
            if (item.Asset == "USDT")
            {
                amount = item.AvailableMargin;
                break;
            }
        }

        return amount;
    }
}