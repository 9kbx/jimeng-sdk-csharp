using JimengSDK;
using JimengSDK.Responses;
using Microsoft.Extensions.Logging;

namespace Samples;

public class JimengAIDemoService
{
    private readonly ILogger<JimengAIDemoService> _logger;
    private readonly IJimengClientFactory _clientFactory;
    private readonly JimengClient _client;

    public JimengAIDemoService(ILogger<JimengAIDemoService> logger, IJimengClientFactory clientFactory)
    {
        _logger = logger;
        _clientFactory = clientFactory;
        _client = _clientFactory.CreateClient();
    }

    public async Task RunDemoAsync()
    {
        await FromLocalFileAsync();
        await FromHttpUrlAsync();
    }

    private async Task FromLocalFileAsync()
    {
        // 使用本地图片
        var filePath = "/Users/j/Downloads/shuiyin3.jpg";
        var base64 = ImageConverter.ImageFileToBase64(filePath);
        ICollection<string> base64Images = [$"{base64}"];

        var prompt = "请去除图片里的水印";
        var res = await _client.ImageToImageV3Async(prompt, base64Images, null!);

        // 快捷调用
        // var res = await _client.ImageToImageV3FromFileAsync(prompt, filePath);

        await WaitForTaskAsync(res);
    }

    private async Task FromHttpUrlAsync()
    {
        // 使用在线图片
        var url =
            "https://portal.volccdn.com/obj/volcfe/cloud-universal-doc/upload_6dc837655edb35b90071547bc0b96e9e.png";
        ICollection<string> images = [url];

        var prompt = "把图片中所有人消除, 把红色裙子改成淡蓝色，把天气环境改为下雪天";
        var res = await _client.ImageToImageV3Async(prompt, null!, images);

        // 快捷调用
        // var res = await _client.ImageToImageV3FromUrlAsync(prompt, url);

        await WaitForTaskAsync(res);
    }

    private async Task WaitForTaskAsync(JimengResponse<ImageToImageData> response)
    {
        if (!response.Success)
            throw new Exception(
                $"Jimeng API Error: {response.ResponseMetadata.Error.Code} - {response.ResponseMetadata.Error.Message}");

        var taskId = response.Data.TaskId;
        Console.WriteLine($"taskId = {taskId}");

        while (true)
        {
            await Task.Delay(5000);
            var res = await _client.GetImageToImageV3ResultAsync(taskId, true);

            Console.WriteLine($"is done = {res.Data.IsDone}");

            if (res.Data.IsDone && res.Data.ImageUrls.Any())
            {
                Console.WriteLine($"image url: {res.Data.ImageUrls.First()}");
                break;
            }
        }
    }
}