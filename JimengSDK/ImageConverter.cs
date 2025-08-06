namespace JimengSDK;

public class ImageConverter
{
    /// <summary>
    /// 将本地图片文件转换为Base64字符串。
    /// </summary>
    /// <param name="imagePath">本地图片文件的完整路径。</param>
    /// <returns>转换后的Base64字符串，如果文件不存在或转换失败则返回null。</returns>
    public static string ImageFileToBase64(string imagePath)
    {
        if (!File.Exists(imagePath))
        {
            throw new FileNotFoundException($"Error: The file at path '{imagePath}' was not found.");
        }

        try
        {
            // 读取文件所有字节
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            // 将字节数组转换为Base64字符串
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// 将图片内存流转换为Base64字符串。
    /// </summary>
    /// <param name="imageStream">包含图片数据的内存流。</param>
    /// <returns>转换后的Base64字符串，如果流为null或转换失败则返回null。</returns>
    public static string ImageStreamToBase64(MemoryStream imageStream)
    {
        if (imageStream == null || imageStream.Length == 0)
        {
            throw new Exception("Error: The provided MemoryStream is null or empty.");
        }

        try
        {
            // 重置流的位置到开头，确保能从头开始读取
            imageStream.Position = 0;
            // 将流转换为字节数组
            byte[] imageBytes = imageStream.ToArray();
            // 将字节数组转换为Base64字符串
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}