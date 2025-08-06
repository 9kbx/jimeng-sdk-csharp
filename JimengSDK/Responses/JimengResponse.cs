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
    
    public ResponseMetadata ResponseMetadata { get; set; }

    public bool Success => Code == 10000;
}

public class ResponseMetadata
{
    public string RequestId { get; set; }
    public string Action { get; set; }
    public string Version { get; set; }
    public string Service { get; set; }
    public string Region { get; set; }
    public Error Error { get; set; }
}

public class Error
{
    public int CodeN { get; set; }
    public string Code { get; set; }
    public string Message { get; set; }
}


