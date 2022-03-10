namespace TheBlogs.API;

public static class GetWriterBlogs
{
    [FunctionName("GetWriterBlogs")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "writer/{writerId:guid}/blogs")] HttpRequest req, Guid writerId,
        [CosmosDB(
            databaseName: "db-stein",
            containerName: "BlogCollection",
            Connection = "ConnectionStrings:Cosmos"
        )] CosmosClient cosmosClient,
        ILogger log)
    {
        var manager = new WriterManager(cosmosClient);

        var blogs = await manager.ListBlogsAsync(writerId.ToString());
        var response = new ApiResponse<List<Blog>>()
        {
            StatusCode = HttpStatusCode.OK,
            Data = blogs
        };
        return new OkObjectResult(response);
    }
}
