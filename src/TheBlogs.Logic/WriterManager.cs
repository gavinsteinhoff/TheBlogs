

using Microsoft.Azure.Cosmos;
using TheBlogs.Models;

namespace TheBlogs.Logic
{
    public class WriterManager
    {
        private const string DatabaseId = "db-stein";
        private const string ContainerId = "WriterCollection";
        private const string BlogContainerId = "BlogCollection";

        private CosmosClient _client;
        private Container _container;
        private Container _blogContainer;

        public WriterManager(CosmosClient cosmosClient)
        {
            _client = cosmosClient;
            _container = _client.GetContainer(DatabaseId, ContainerId);
            _blogContainer = _client.GetContainer(DatabaseId, BlogContainerId);
        }

        public async Task<Writer> GetAsync(string itemId)
        {
            var requestOptions = new QueryRequestOptions()
            {
                MaxConcurrency = 1,
                MaxItemCount = 1
            };
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                .WithParameter("@id", itemId);

            var items = new List<Writer>();
            var queryResultSetIterator = _container.GetItemQueryIterator<Writer>(queryDefinition: query, requestOptions: requestOptions);
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

        public async Task<List<Writer>> ListAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var items = new List<Writer>();
            var queryResultSetIterator = _container.GetItemQueryIterator<Writer>(queryDefinition: query);
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

        public async Task<List<Blog>> ListBlogsAsync(string writerId)
        {
            var requestOptions = new QueryRequestOptions()
            {
                PartitionKey = new PartitionKey(writerId),
            };
            var query = new QueryDefinition("SELECT * FROM c");
            var items = new List<Blog>();
            var queryResultSetIterator = _blogContainer.GetItemQueryIterator<Blog>(queryDefinition: query, requestOptions: requestOptions);
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
    }
}
