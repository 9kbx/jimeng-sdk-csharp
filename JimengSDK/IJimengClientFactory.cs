using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JimengSDK;

public interface IJimengClientFactory
{
    JimengClient CreateClient();
    JimengClient CreateClient(string accessKeyId, string accessKeySecret);
}

public class JimengClientFactory : IJimengClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JimengOptions _defaultOptions;
    private readonly ILogger<JimengClient> _logger;

    // 工厂本身依赖于 IHttpClientFactory 和默认的 JiMengAIOptions
    public JimengClientFactory(IHttpClientFactory httpClientFactory, IOptions<JimengOptions> defaultOptions,
        ILogger<JimengClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _defaultOptions = defaultOptions.Value;
        _logger = logger;
    }

    public JimengClient CreateClient() => CreateClient(_defaultOptions.AccessKeyId, _defaultOptions.AccessKeySecret);

    public JimengClient CreateClient(string accessKeyId, string accessKeySecret)
    {
        // 复制默认配置，然后覆盖动态传入的配置
        var options = new JimengOptions
        {
            Endpoint = _defaultOptions.Endpoint,
            Path = _defaultOptions.Path,
            Service = _defaultOptions.Service,
            Region = _defaultOptions.Region,
            Schema = _defaultOptions.Schema,
            AccessKeyId = accessKeyId, // 动态传入
            AccessKeySecret = accessKeySecret // 动态传入
        };

        // 创建一个新的 HttpClient 实例
        var httpClient = _httpClientFactory.CreateClient("JimengAIClient");

        // 使用新的配置和 HttpClient 实例创建 JiMengAIClient
        return new JimengClient(options, httpClient, _logger);
    }
}