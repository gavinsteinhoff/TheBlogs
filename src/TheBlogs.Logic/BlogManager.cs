

using Microsoft.Azure.Cosmos;
using TheBlogs.Models;

namespace TheBlogs.Logic
{
    public class BlogManager
    {
        private const string DatabaseId = "db-stein";
        private const string BlogContainerId = "BlogCollection";

        private CosmosClient _client;
        private Container _container;

        public BlogManager(CosmosClient cosmosClient)
        {
            _client = cosmosClient;
            _container = _client.GetContainer(DatabaseId, BlogContainerId);
        }

        public async Task<Blog> AddAsync(Blog blog)
        {
            if (await DoesTitleExist(blog.Title, blog.WriterId))
            {
                return null;
            }

            var blogResponse = await _container.CreateItemAsync(blog, new PartitionKey(blog.WriterId));

            return blogResponse.Resource;
        }

        public async Task<Blog> GetAsync(string itemId)
        {
            var requestOptions = new QueryRequestOptions()
            {
                MaxConcurrency = 1,
                MaxItemCount = 1
            };
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                .WithParameter("@id", itemId);

            var items = new List<Blog>();
            var queryResultSetIterator = _container.GetItemQueryIterator<Blog>(queryDefinition: query, requestOptions: requestOptions);
            while (queryResultSetIterator.HasMoreResults)
            {
               var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (var item in currentResultSet)
                {
                    items.Add(item);
                }
            }

            if (items.Count > 0) return items[0];

            return null;
        }

        public async Task<List<Blog>> ListAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var items = new List<Blog>();
            var queryResultSetIterator = _container.GetItemQueryIterator<Blog>(queryDefinition: query);
            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (var item in currentResultSet)
                {
                    items.Add(item);
                }
            }
            return items;
        }

        private async Task<bool> DoesTitleExist(string title, string writerId)
        {
            var requestOptions = new QueryRequestOptions()
            {
                PartitionKey = new PartitionKey(writerId),
                MaxConcurrency = 1,
                MaxItemCount = 1
            };

            var query = new QueryDefinition("SELECT * FROM c WHERE c.title = @title")
                .WithParameter("@title", title);

            var blogs = new List<Blog>();

            var queryResultSetIterator = _container.GetItemQueryIterator<Blog>(queryDefinition: query, requestOptions: requestOptions);
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Blog> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (var blog in currentResultSet)
                {
                    blogs.Add(blog);
                }
            }

            return blogs.Count > 0;
        }

    }
}
