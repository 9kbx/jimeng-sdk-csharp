using System.Text.Json.Serialization;

namespace JimengSDK.Responses;

public class GetImageToImageResultData
{
    /// <summary>
    /// 返回图片的base64数组。
    /// </summary>
    [JsonPropertyName("binary_data_base64")]
    public ICollection<string>? BinaryDataBase64 { get; set; } = [];

    /// <summary>
    /// 返回图片的url数组
    /// </summary>
    [JsonPropertyName("image_urls")]
    public ICollection<string>? ImageUrls { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("resp_data")]
    public string RespData { get; set; } = string.Empty;

    /// <summary>
    /// 任务执行状态
    /// </summary>
    /// <remarks>
    /// in_queue：任务已提交 <br/>
    /// generating：任务已被消费，处理中 <br/>
    /// done：处理完成，成功或者失败，可根据外层code与message进行判断 <br/>
    /// not_found：任务未找到，可能原因是无此任务或任务已过期(12小时) <br/>
    /// expired：任务已过期，请尝试重新提交任务请求
    /// </remarks>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public bool IsDone => !string.IsNullOrEmpty(Status) && Status == "done";
}