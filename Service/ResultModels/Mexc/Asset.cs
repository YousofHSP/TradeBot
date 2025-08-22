namespace Application.ResultModels.Mexc;

public class Asset
{
    public string Currency { get; set; }
    public decimal PositionMargin { get; set; }
    public decimal FrozenBalance { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal CashBalance { get; set; }
    public decimal Equity { get; set; }
    public decimal Unrealized { get; set; }
}