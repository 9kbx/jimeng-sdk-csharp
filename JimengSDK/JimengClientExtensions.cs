using JimengSDK.Responses;

namespace JimengSDK;

public static class JimengClientExtensions
{
    public static Task<JimengResponse<ImageToImageData>> ImageToImageV3FromUrlAsync(this JimengClient client,
        string prompt, string imageUrl, int width = 1024, int height = 1024, int seed = -1, float scale = 0.5f)
    {
        ICollection<string> imageUrls = [imageUrl];
        return client.ImageToImageV3Async(prompt, null!, imageUrls, width, height, seed, scale);
    }

    public static Task<JimengResponse<ImageToImageData>> ImageToImageV3FromFileAsync(this JimengClient client,
        string prompt, string filePath, int width = 1024, int height = 1024, int seed = -1, float scale = 0.5f)
    {
        var base64 = ImageConverter.ImageFileToBase64(filePath);
        ICollection<string> base64Images = [$"{base64}"];

        return client.ImageToImageV3Async(prompt, base64Images, null!, width, height, seed, scale);
    }
}