# 即梦AI的C#版SDK

## 已实现接口
- 图生图3.0

## 使用示例

配置密钥

```json
{
  "JimengAI": {
    "AccessKeyId": "AKxxx",
    "AccessKeySecret": "xxxx"
  }
}
```

注册即梦API客户端

```csharp
services.AddJiMengAIClient();
```

创建图生图任务

```csharp
public class DemoService(IJimengClientFactory clientFactory)
{
    public async Task RunAsync()
    {
        var client = clientFactory.CreateClient();

        var prompt = "把图片中所有人消除, 把红色裙子改成淡蓝色，把天气环境改为下雪天";
        ICollection<string> images =
            ["https://portal.volccdn.com/obj/volcfe/cloud-universal-doc/upload_6dc837655edb35b90071547bc0b96e9e.png"];
        
        // 创建图生图任务
        var res1 = await client.ImageToImageV3Async(prompt, null, images);
        
        // 或者用扩展方法直接调用
        var res = await _client.ImageToImageV3FromUrlAsync(prompt, url);
        
        Console.WriteLine($"taskId = {res1.Data.TaskId}");

        var res2 = await client.GetImageToImageV3ResultAsync(res1.Data.TaskId, true);
        
        Console.WriteLine($"is done = {res2.Data.IsDone}");
        if (res2.Data.IsDone && res2.Data.ImageUrls.Any())
        {
            Console.WriteLine($"image url: {res2.Data.ImageUrls.First()}");
        }
    }
}
```