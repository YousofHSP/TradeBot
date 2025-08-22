namespace Application.ResultModels.Mexc;

public class SymbolResult
{
    public List<SymbolItem> Symbols { get; set; } = [];
}

public class SymbolItem
{
    public string Symbol { get; set; }
    public string BaseAsset{ get; set; }
    public decimal LastPrice{ get; set; }
    public decimal Price { get; set; }
}