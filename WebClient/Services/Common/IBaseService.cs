namespace WebClient.Services.Common;

public interface IBaseService
{
    Task<TResDto?> Post<TDto, TResDto>(string uri, TDto dto) where TResDto : class, new();
    Task<TResDto?> Patch<TDto, TResDto>(string uri, TDto dto) where TResDto : class, new();
    Task<TResDto?> Get<TResDto>(string uri) where TResDto : class;
    Task<bool> Delete(string uri);
    HttpClient GetHttpClient();
}