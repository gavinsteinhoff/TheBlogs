using System.Text.Json.Serialization;

namespace TheBlogs.Models;

public class Writer
{
    [JsonPropertyName("id")]
    public string Id { get; } = Guid.NewGuid().ToString();
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    public List<Blog> Blogs { get; set; } = new List<Blog>();
}
