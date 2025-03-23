namespace PaginatedApi;

public interface IReviewService
{
    Task<ReviewDto?> GetBestReviewAsync(string city, CancellationToken cancellationToken = default);
}
