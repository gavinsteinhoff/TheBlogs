using System.Net;

namespace TheBlogs.Models;

public class ApiResponse<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public T? Data { get; set; }
}
