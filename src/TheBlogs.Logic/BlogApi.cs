using System.Net.Http.Json;

namespace TheBlogs.Logic;

public interface IBlogApi
{
    Task SubmitBlogAsync(Blog blog);
}

public class BlogApi : IBlogApi
{
    private readonly HttpClient _httpClient;

    public BlogApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task SubmitBlogAsync(Blog blog)
    {
        var apiResponse = await _httpClient.PostAsJsonAsync("http://localhost:7071/api/blogs", blog);
    }
}
