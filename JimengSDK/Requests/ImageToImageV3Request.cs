using System.Text.Json;

namespace JimengSDK.Requests;

public static class ImageToImageV3Request
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="prompt">用于编辑图像的提示词</param>
    /// <param name="binaryDataBase64">图片文件base64编码，需输入1张图片; 与url二选一</param>
    /// <param name="imageUrls">图片文件URL，需输入1张图片；与base64data二选一</param>
    /// <param name="width">生成图像宽高，系统默认生成1328 * 1328的图像,支持自定义生成图像宽高，范围在[512, 2016]内；推荐可选的宽高比：1328 * 1328（1:1）/1472 * 1104 （4:3）/1584 * 1056（3:2）/1664 * 936（16:9）/2016 * 864（21:9）</param>
    /// <param name="height">生成图像宽高，需同时传width和height才会生效；如果自定义生图宽高都比1024小很多（如：600以下）可能出图全黑，建议优先设置接近1024的生图宽高； <br/>最终输出图宽高与传入宽高相关但不完全相等，为“与传入宽高最接近16整数倍”的像素值，范围在 [512, 1536] 内；</param>
    /// <param name="seed">随机种子，作为确定扩散初始状态的基础，默认-1（随机）。若随机种子为相同正整数且其他参数均一致，则生成内容极大概率效果一致</param>
    /// <param name="scale">文本描述影响的程度，该值越大代表文本描述影响程度越大，且输入图片影响程度越小<br/>默认值：0.5<br/>取值范围：[0, 1]</param>
    /// <returns></returns>
    public static byte[] GetRequestBodyBytes(string prompt, ICollection<string> binaryDataBase64,
        ICollection<string> imageUrls,
        int width = 1024, int height = 1024, int seed = -1, float scale = 0.5f) => JsonSerializer.SerializeToUtf8Bytes(
        GetRequestBody(prompt, binaryDataBase64, imageUrls, width, height, seed,
            scale));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prompt">用于编辑图像的提示词</param>
    /// <param name="binaryDataBase64">图片文件base64编码，需输入1张图片; 与url二选一</param>
    /// <param name="imageUrls">图片文件URL，需输入1张图片；与base64data二选一</param>
    /// <param name="width">生成图像宽高，系统默认生成1328 * 1328的图像,支持自定义生成图像宽高，范围在[512, 2016]内；推荐可选的宽高比：1328 * 1328（1:1）/1472 * 1104 （4:3）/1584 * 1056（3:2）/1664 * 936（16:9）/2016 * 864（21:9）</param>
    /// <param name="height">生成图像宽高，需同时传width和height才会生效；如果自定义生图宽高都比1024小很多（如：600以下）可能出图全黑，建议优先设置接近1024的生图宽高； <br/>最终输出图宽高与传入宽高相关但不完全相等，为“与传入宽高最接近16整数倍”的像素值，范围在 [512, 1536] 内；</param>
    /// <param name="seed">随机种子，作为确定扩散初始状态的基础，默认-1（随机）。若随机种子为相同正整数且其他参数均一致，则生成内容极大概率效果一致</param>
    /// <param name="scale">文本描述影响的程度，该值越大代表文本描述影响程度越大，且输入图片影响程度越小<br/>默认值：0.5<br/>取值范围：[0, 1]</param>
    /// <returns></returns>
    public static IDictionary<string, object> GetRequestBody(string prompt, ICollection<string> binaryDataBase64,
        ICollection<string> imageUrls,
        int width = 1024, int height = 1024, int seed = -1, float scale = 0.5f)
    {
        if (string.IsNullOrEmpty(prompt))
            throw new ArgumentNullException(nameof(prompt));
        if (binaryDataBase64 == null && imageUrls == null)
            throw new Exception($"参数{nameof(binaryDataBase64)},{nameof(imageUrls)}必须有一个不能为空");

        if (width < 512) width = 512;
        if (height < 512) height = 512;
        var data = new Dictionary<string, object>
        {
            { "req_key", "jimeng_i2i_v30" },
            { "prompt", prompt },
            { "width", width },
            { "height", height },
            { "seed", seed },
            { "scale", scale },
        };
        if (binaryDataBase64 != null && binaryDataBase64.Any())
        {
            data.Add("binary_data_base64", binaryDataBase64);
        }
        else if (imageUrls != null && imageUrls.Any())
        {
            data.Add("image_urls", imageUrls);
        }

        return data;
    }
}