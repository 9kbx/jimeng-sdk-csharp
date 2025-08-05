using System.Text.Json.Serialization;

namespace JimengSDK.Responses;

public class JimengResponse<TData>
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    
    [JsonPropertyName("data")]
    public TData Data { get; set; }
    
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
    
    [JsonPropertyName("request_id")]
    public string RequestId { get; set; } = string.Empty;
    
    [JsonPropertyName("status")]
    public int Status { get; set; }
    
    [JsonPropertyName("time_elapsed")]
    public string TimeElapsed { get; set; } = string.Empty;
}

