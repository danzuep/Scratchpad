namespace PaginatedApi;

public class ReviewDto
{
    public string Name { get; set; }
    public double Rating { get; set; }
    public int NumberOfVotes { get; set; }
}

public class ApiResponse
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public List<RestaurantData> Data { get; set; }
}

public class RestaurantData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public VoteData VoteData { get; set; }
}

public class VoteData
{
    public double Rating { get; set; }
    public int NumberOfVotes { get; set; }
}