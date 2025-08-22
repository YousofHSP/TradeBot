namespace Application.ResultModels.Mexc;

public class Result<TData>
{
    public bool Success { get; set; }
    public int Code { get; set; }
    public TData Data { get; set; }
    
}