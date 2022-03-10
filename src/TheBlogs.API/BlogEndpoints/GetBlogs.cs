namespace TheBlogs.API;

public static class GetBlogs
{
    [FunctionName("GetBlogs")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "blogs/{blogId:guid?}")] HttpRequest req, Guid? blogId,
        [CosmosDB(
            databaseName: "db-stein",
            containerName: "BlogCollection",
            Connection = "ConnectionStrings:Cosmos"
        )] CosmosClient cosmosClient,
        ILogger log)
    {
        var blogManager = new BlogManager(cosmosClient);


        if (blogId == null)
            return new OkObjectResult(new ApiResponse<List<Blog>>()
            {
                StatusCode = HttpStatusCode.OK,
                Data = await blogManager.ListAsync()
            });
        else
            return new OkObjectResult(new ApiResponse<Blog>()
            {
                StatusCode = HttpStatusCode.OK,
                Data = await blogManager.GetAsync(blogId.ToString())

            });
    }
}
