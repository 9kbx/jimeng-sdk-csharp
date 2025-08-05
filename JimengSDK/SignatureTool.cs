using System.Collections;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace JimengSDK;

public class SignatureTool
{
    private static readonly BitArray UrlEncoder = new(256);
    private static readonly string ConstEncode = "0123456789ABCDEF";


    public HttpRequestMessage CreateHttpRequestMessage(
        string method, IDictionary<string, string> queryList,
        byte[] body, DateTimeOffset date, string action, string version)
    {
        // 如果body为空，则初始化一个空数组
        body ??= new byte[0];

        // C#中将字节数组转换为十六进制字符串
        string xContentSha256 = HashSHA256(body);

        // 使用DateTime.ToString()进行日期格式化
        string xDate = date.ToString("yyyyMMdd'T'HHmmss'Z'");
        string shortXDate = date.ToString("yyyyMMdd");
        string contentType = "application/json";
        string signHeader = "host;x-content-sha256;x-date";

        // 将字典转换为SortedDictionary以进行排序，与Java中的TreeMap相同
        SortedDictionary<string, string> realQueryList = queryList != null && queryList.Any()
            ? new SortedDictionary<string, string>(queryList)
            : new SortedDictionary<string, string>();
        realQueryList.Add("Action", action);
        realQueryList.Add("Version", version);

        // 使用StringBuilder和String.Join构建查询字符串
        var canonicalQueryString = string.Join("&",
            realQueryList.Select(kv => $"{SignStringEncoder(kv.Key)}={SignStringEncoder(kv.Value)}"));

        // 构建规范的请求 CanonicalRequest
        string canonicalStringBuilder = $"{method}\n{path}\n{canonicalQueryString}\n" +
                                        // $"content-type:{contentType}\n" +
                                        $"host:{host}\n" +
                                        $"x-content-sha256:{xContentSha256}\n" +
                                        $"x-date:{xDate}\n" +
                                        $"\n" +
                                        $"{signHeader}\n" +
                                        $"{xContentSha256}";

        // Console.WriteLine("==============  构建规范的请求 CanonicalRequest   =======================");
        // Console.WriteLine(canonicalStringBuilder);

        // 构建待签字符串 StringToSign
        string hashcanonicalString = HashSHA256(Encoding.UTF8.GetBytes(canonicalStringBuilder));
        string credentialScope = $"{shortXDate}/{region}/{service}/request";
        string signString = $"HMAC-SHA256\n{xDate}\n{credentialScope}\n{hashcanonicalString}";

        // Console.WriteLine("============== 构建待签字符串 StringToSign   =======================");
        // Console.WriteLine(hashcanonicalString);

        // 计算签名密钥
        byte[] signKey = GenSigningSecretKeyV4(sk, shortXDate, region, service);
        // 计算签名
        string signature = ByteArrayToHexString(HmacSHA256(signKey, signString));

        // Note: You need to add `using System.Net.Http;` and `using System.IO;` in the file that uses this method.
        // It's recommended to place both files in the same project.
        string url = $"{schema}://{host}{path}?{canonicalQueryString}";

        var request = new HttpRequestMessage(new HttpMethod(method), url);

        // 设置请求头
        request.Headers.Add("host", host);
        request.Headers.Add("x-date", xDate);
        request.Headers.Add("x-content-sha256", xContentSha256);

        var auth = $"Credential={ak}/{credentialScope}, SignedHeaders={signHeader}, Signature={signature}";
        request.Headers.Authorization = new AuthenticationHeaderValue("HMAC-SHA256", auth);

        // 如果请求方法不是GET，则设置请求体
        if (method != "GET")
        {
            request.Content = new StringContent(Encoding.UTF8.GetString(body), Encoding.UTF8, contentType);
        }

        return request;
    }

    // 使用静态构造函数初始化UrlEncoder，与Java的静态代码块功能相同。
    static SignatureTool()
    {
        for (int i = 97; i <= 122; i++) // a-z
        {
            UrlEncoder[i] = true;
        }

        for (int i = 65; i <= 90; i++) // A-Z
        {
            UrlEncoder[i] = true;
        }

        for (int i = 48; i <= 57; i++) // 0-9
        {
            UrlEncoder[i] = true;
        }

        UrlEncoder[(int)'-'] = true;
        UrlEncoder[(int)'_'] = true;
        UrlEncoder[(int)'.'] = true;
        UrlEncoder[(int)'~'] = true;
    }

    private readonly string region;
    private readonly string service;
    private readonly string schema;
    private readonly string host;
    private readonly string path;
    private readonly string ak;
    private readonly string sk;

    public SignatureTool(string region, string service, string schema, string host, string path, string ak,
        string sk)
    {
        this.region = region;
        this.service = service;
        this.host = host;
        this.schema = schema;
        this.path = path;
        this.ak = ak;
        this.sk = sk;
    }

    /// <summary>
    /// 自定义URL编码器，与Java版本中的逻辑相同。
    /// </summary>
    private string SignStringEncoder(string source)
    {
        if (source == null)
        {
            return null;
        }

        var buf = new StringBuilder();
        byte[] bytes = Encoding.UTF8.GetBytes(source);

        foreach (byte b in bytes)
        {
            if (UrlEncoder[b])
            {
                buf.Append((char)b);
            }
            else if (b == 32)
            {
                buf.Append("%20");
            }
            else
            {
                buf.Append('%');
                char hex1 = ConstEncode[b >> 4];
                char hex2 = ConstEncode[b & 15];
                buf.Append(hex1);
                buf.Append(hex2);
            }
        }

        return buf.ToString();
    }

    private static string HashSHA256(byte[] content)
    {
        using var sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(content);
        return ByteArrayToHexString(hash);
    }

    private static byte[] HmacSHA256(byte[] key, string content)
    {
        using var hmac = new HMACSHA256(key);
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(content));
        return hash;
    }

    private byte[] GenSigningSecretKeyV4(string secretKey, string date, string region, string service)
    {
        byte[] kDate = HmacSHA256(Encoding.UTF8.GetBytes(secretKey), date);
        byte[] kRegion = HmacSHA256(kDate, region);
        byte[] kService = HmacSHA256(kRegion, service);
        return HmacSHA256(kService, "request");
    }

    /// <summary>
    /// 将字节数组转换为十六进制字符串，与Java的HexFormat.of().formatHex()功能相同。
    /// </summary>
    private static string ByteArrayToHexString(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
    }
}