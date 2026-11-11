using System.Text;
using System.Text.Json;

namespace RaceDay.MVC.Services;

public class ApiClient
{
    private readonly HttpClient _client;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _client = httpClientFactory.CreateClient("RaceDayApi");
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        var response = await _client.GetAsync(url);
        if (!response.IsSuccessStatusCode) return default;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<HttpResponseMessage> PostAsync(string url, object data)
    {
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        return await _client.PostAsync(url, content);
    }

    public async Task<HttpResponseMessage> UploadFileAsync(string url, IFormFile file)
    {
        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
        return await _client.PostAsync(url, content);
    }
}
