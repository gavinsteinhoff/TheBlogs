using Microsoft.AspNetCore.Http;

namespace TheBlogs.API;

public static class PostBlog
{
    [FunctionName("PostBlog")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "blogs")] HttpRequest req,
        [CosmosDB(
            databaseName: "db-stein",
            containerName: "BlogCollection",
            Connection = "ConnectionStrings:Cosmos"
        )] CosmosClient cosmosClient,
        ILogger log)
    {
        try
        {
            var blogManager = new BlogManager(cosmosClient);

            string requestBody = string.Empty;
            using (var streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            var data = JsonSerializer.Deserialize<Blog>(requestBody);

            var blog = new Blog()
            {
                WriterId = "3db97735-9c0c-4a78-9517-ab901d18df92",
                Title = data.Title,
                Text = data.Text,
            };

            var response = await blogManager.AddAsync(blog);

            return new OkObjectResult(new ApiResponse<Blog>()
            {
                StatusCode = HttpStatusCode.OK,
                Data = response
            });
        }
        catch (Exception ex)
        {
            return new ObjectResult(new ApiResponse<Blog>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Data = { },
                ErrorMessage = ex.Message
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
