using System.Text.Json;
using JimengSDK.Requests;
using JimengSDK.Responses;
using Microsoft.Extensions.Logging;

namespace JimengSDK;

/// <summary>
/// 即梦AI接口
/// </summary>
/// <remarks>官方文档：https://www.volcengine.com/docs/85621/1747301</remarks>
public class JimengClient
{
    private readonly ILogger<JimengClient> _logger;
    private readonly HttpClient _httpClient;
    private readonly JimengOptions _options;
    private readonly SignatureTool _signatureTool;

    public JimengClient(JimengOptions options, HttpClient httpClient, ILogger<JimengClient> logger)
    {
        _options = options;
        _httpClient = httpClient;
        _logger = logger;

        _signatureTool = new SignatureTool(_options.Region, _options.Service, _options.Schema, _options.Endpoint,
            _options.Path, _options.AccessKeyId, _options.AccessKeySecret);
    }

    /// <summary>
    /// 创建图生图(3.0)任务
    /// </summary>
    /// <param name="prompt"></param>
    /// <param name="binaryDataBase64"></param>
    /// <param name="imageUrls"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="seed"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public async Task<JimengResponse<ImageToImageData>> ImageToImageV3Async(string prompt,
        ICollection<string> binaryDataBase64,
        ICollection<string> imageUrls, int width = 1024, int height = 1024, int seed = -1, float scale = 0.5f)
    {
        var body = ImageToImageV3Request.GetRequestBodyBytes(prompt, binaryDataBase64, imageUrls, width, height, seed,
            scale);
        var queryList = new SortedDictionary<string, string>();
        // queryList.Add("X-Expires","900"); // 签名的有效时间(可选)，单位为秒，不填时默认值为900。

        using var httpRequest = _signatureTool.CreateHttpRequestMessage("POST", queryList, body, DateTimeOffset.UtcNow,
            "CVSync2AsyncSubmitTask",
            "2022-08-31");

        var response = await _httpClient.SendAsync(httpRequest);
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<JimengResponse<ImageToImageData>>(responseBody);
    }

    /// <summary>
    /// 获取图生图(3.0)任务状态
    /// </summary>
    /// <param name="taskId">任务ID</param>
    /// <param name="returnUrl"></param>
    /// <param name="addLogo"></param>
    /// <param name="position"></param>
    /// <param name="language"></param>
    /// <param name="opacity"></param>
    /// <param name="logoTextContent"></param>
    /// <returns></returns>
    public async Task<JimengResponse<GetImageToImageResultData>> GetImageToImageV3ResultAsync(
        string taskId, bool returnUrl = false, bool addLogo = false,
        int position = 0, int language = 0, float opacity = 0.3f, string logoTextContent = "")
    {
        var body = GetImageToImageV3ResultRequest.ToRequestBodyBytes(taskId, returnUrl, addLogo, position, language,
            opacity, logoTextContent);
        var queryList = new SortedDictionary<string, string>();
        // queryList.Add("X-Expires","900"); // 签名的有效时间(可选)，单位为秒，不填时默认值为900。
        using var httpRequest = _signatureTool.CreateHttpRequestMessage("POST", queryList, body, DateTimeOffset.UtcNow,
            "CVSync2AsyncGetResult",
            "2022-08-31");

        var response = await _httpClient.SendAsync(httpRequest);
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<JimengResponse<GetImageToImageResultData>>(responseBody);
    }
}