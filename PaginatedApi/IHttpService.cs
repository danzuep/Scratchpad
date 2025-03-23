namespace PaginatedApi
{
    public interface IHttpService
    {
        Task<ApiResponse?> FetchReviewsAsync(string resource, CancellationToken cancellationToken = default);
    }
}