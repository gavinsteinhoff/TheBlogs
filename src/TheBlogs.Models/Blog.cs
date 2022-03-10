using System.Text.Json.Serialization;

namespace TheBlogs.Models;

public class Blog
{
    [JsonPropertyName("id")]
    public string Id { get; } = Guid.NewGuid().ToString();
    [JsonPropertyName("writerId")]
    public string WriterId { init; get; } = string.Empty;
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}
