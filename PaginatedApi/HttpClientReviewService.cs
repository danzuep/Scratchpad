using System.Net.Http.Json;

namespace PaginatedApi;

public class HttpClientReviewService : IHttpService
{
    private readonly HttpClient _httpClient;

    public HttpClientReviewService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse?> FetchReviewsAsync(string resource, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<ApiResponse>(resource, cancellationToken);
    }
}
