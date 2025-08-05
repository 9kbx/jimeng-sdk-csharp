namespace JimengSDK;

public class JimengOptions
{
    public string Endpoint { get; set; } = "visual.volcengineapi.com";
    public string Path { get; set; } = "/";
    public string Service { get; set; } = "cv";
    public string Region { get; set; } = "cn-north-1";
    public string Schema { get; set; } = "https";
    public string AccessKeyId { get; set; } = string.Empty;
    public string AccessKeySecret { get; set; } = string.Empty;
}