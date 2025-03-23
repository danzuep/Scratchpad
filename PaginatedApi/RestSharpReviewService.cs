using Newtonsoft.Json;
using RestSharp;

namespace PaginatedApi;

public class RestSharpReviewService : IHttpService
{
    private readonly RestClient _restClient;

    public RestSharpReviewService(RestClient restClient)
    {
        _restClient = restClient;
    }

    public async Task<ApiResponse?> FetchReviewsAsync(string resource, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest(resource, Method.Get);
        var response = await _restClient.ExecuteAsync(request, cancellationToken);

        if (!response.IsSuccessful || response.Content == null)
            return null;

        return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
    }
}
