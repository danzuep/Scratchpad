namespace PaginatedApi;

public class ReviewService : IReviewService
{
    private readonly IHttpService _httpService;

    public ReviewService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<ReviewDto?> GetBestReviewAsync(string city, CancellationToken cancellationToken = default)
    {
        ReviewDto? bestReview = null;

        for (int pageNumber = 1; ; pageNumber++)
        {
            var resource = $"?city={city}&page={pageNumber}";
            var response = await _httpService.FetchReviewsAsync(resource, cancellationToken);

            if (response == null || !response.Data.Any())
                break;

            foreach (var restaurant in response.Data)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var newDto = ToDto(restaurant);
                bestReview = GetBetterReview(bestReview, newDto);
            }

            if (response.CurrentPage >= response.TotalPages)
                break;
        }

        return bestReview;
    }

    private ReviewDto ToDto(RestaurantData restaurant)
    {
        return new ReviewDto
        {
            Name = restaurant.Name,
            Rating = restaurant.VoteData.Rating,
            NumberOfVotes = restaurant.VoteData.NumberOfVotes
        };
    }

    private ReviewDto GetBetterReview(ReviewDto? currentBest, ReviewDto newReview)
    {
        if (currentBest == null)
            return newReview;

        if (newReview.Rating > currentBest.Rating ||
            (newReview.Rating == currentBest.Rating && newReview.NumberOfVotes > currentBest.NumberOfVotes))
        {
            return newReview;
        }

        return currentBest;
    }
}
