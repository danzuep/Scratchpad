using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace PaginatedApi;

internal class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var baseUrl = configuration["BaseUrl"] ?? throw new ArgumentNullException("BaseUrl");
        var city = configuration["City"] ?? throw new ArgumentNullException("City");
        var reviewService = GetReviewService(baseUrl);
        await GetBestReviewAsync(reviewService, city);
    }

    static IReviewService GetReviewService(string baseUrl)
    {
        ArgumentNullException.ThrowIfNull(baseUrl, nameof(baseUrl));
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection, baseUrl);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var reviewService = serviceProvider.GetRequiredService<IReviewService>();
        return reviewService;
    }

    static async Task GetBestReviewAsync(IReviewService reviewService, string city)
    {
        var bestReview = await reviewService.GetBestReviewAsync(city);
        if (bestReview != null)
        {
            Debug.WriteLine($"Name: {bestReview.Name}, Rating: {bestReview.Rating}, Votes: {bestReview.NumberOfVotes}");
            Console.WriteLine(bestReview.Name);
        }
    }

    static void ConfigureServices(IServiceCollection services, string baseUrl)
    {
        services.AddSingleton(new RestClient(baseUrl)); // superceded by HttpClient
        services.AddSingleton<IHttpService, RestSharpReviewService>();
        services.AddSingleton(new HttpClient { BaseAddress = new Uri(baseUrl) });
        services.AddSingleton<IHttpService, HttpClientReviewService>();
        services.AddSingleton<IReviewService, ReviewService>();
    }
}
