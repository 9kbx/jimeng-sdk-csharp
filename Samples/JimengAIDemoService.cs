using JimengSDK;
using Microsoft.Extensions.Logging;

namespace Samples;

public class JimengAIDemoService
{
    private readonly ILogger<JimengAIDemoService> _logger;
    private readonly IJimengClientFactory _clientFactory;

    public JimengAIDemoService(ILogger<JimengAIDemoService> logger, IJimengClientFactory clientFactory)
    {
        _logger = logger;
        _clientFactory = clientFactory;
    }

    public async Task RunDemoAsync()
    {
        var client = _clientFactory.CreateClient();

        var prompt = "把图片中所有人消除, 把红色裙子改成淡蓝色，把天气环境改为下雪天";
        ICollection<string> images =
            ["https://portal.volccdn.com/obj/volcfe/cloud-universal-doc/upload_6dc837655edb35b90071547bc0b96e9e.png"];
        var res1 = await client.ImageToImageV3Async(prompt, null, images);

        var taskId = res1.Data.TaskId;
        Console.WriteLine($"taskId = {taskId}");

        while (true)
        {
            await Task.Delay(5000);
            // var taskId = "514842385226612206";
            var res2 = await client.GetImageToImageV3ResultAsync(taskId, true);

            Console.WriteLine($"is done = {res2.Data.IsDone}");

            if (res2.Data.IsDone && res2.Data.ImageUrls.Any())
            {
                Console.WriteLine($"image url: {res2.Data.ImageUrls.First()}");
                break;
            }
        }
    }
}