using Microsoft.Extensions.DependencyInjection;

namespace JimengSDK;

public static class JimengExtensions
{
    public static void AddJimengClient(this IServiceCollection services, Action<JimengOptions> options = null)
    {
        // 注册一个命名的 HttpClient，供工厂使用
        services.AddHttpClient("JimengAIClient", client =>
        {
            client.BaseAddress = new Uri("https://visual.volcengineapi.com/");
            client.DefaultRequestHeaders.Accept.Clear();
        });

        // 注册默认的配置
        services.AddOptions<JimengOptions>()
            .BindConfiguration("JimengAI");
        // services.Configure(options ?? (_ => { }));
        if (options != null)
        {
            services.Configure(options);
        }
        // 注册客户端工厂
        services.AddSingleton<IJimengClientFactory, JimengClientFactory>();
    }
}