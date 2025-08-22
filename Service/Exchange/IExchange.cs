using Domain.Entities;
using Shared.DTOs;

namespace Service.Exchange;

public interface IExchange
{
    public Task<List<CoinResDto>> GetCoins(string? currency,CancellationToken ct);
    public Task<CoinResDto?> GetCoin(string currency, CancellationToken ct);
    public Task<long> PlaceOrder(Coin coin, int leverage, CancellationToken ct);
    public Task<decimal> GetCoinPrice(Coin coin, CancellationToken ct);
    public Task<decimal> GetBalance(string currency = "USDT", CancellationToken cancellationToken = default);
}