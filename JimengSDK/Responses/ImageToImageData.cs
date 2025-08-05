using System.Text.Json.Serialization;

namespace JimengSDK.Responses;

public class ImageToImageData
{
    /// <summary>
    /// 任务ID
    /// </summary>
    [JsonPropertyName("task_id")]
    public string TaskId { get; set; } = string.Empty;
}