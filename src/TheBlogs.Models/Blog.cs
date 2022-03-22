using Markdig;
using Markdig.Extensions.MudBlazor;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TheBlogs.Models;

public class Blog
{
    [JsonPropertyName("id")]
    [JsonProperty("id")]
    public string Id { get; init; } = Guid.NewGuid().ToString();

    [JsonPropertyName("writerId")]
    [JsonProperty("writerId")]
    public string WriterId { init; get; } = string.Empty;

    [JsonPropertyName("title")]
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("createdDate")]
    [JsonProperty("createdDate")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [JsonPropertyName("modifiedDate")]
    [JsonProperty("modifiedDate")]
    public DateTime ModifiedDate { get; set; } = DateTime.Now;

    [JsonPropertyName("text")]
    [JsonProperty("text")]
    public string Text { get; set; } = string.Empty;

    public string TextFormated => Markdown.ToHtml(Text, _pipeline);

    private readonly MarkdownPipeline _pipeline;
    public Blog()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseMudBlazorExtension()
            .Build();
    }


}
