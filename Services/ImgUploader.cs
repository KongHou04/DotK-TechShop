namespace DotK_TechShop.Services;

public class ImageUploader
{
    private readonly string _targetDirectory;

    public ImageUploader(string targetDirectory)
    {
        _targetDirectory = targetDirectory;
    }

    public string GetImagePath(string imageName)
    {
        try
        {
            if (string.IsNullOrEmpty(imageName))
            {
                throw new ArgumentException("Image name is empty.");
            }

            string imagePath = Path.Combine(_targetDirectory, imageName);
            return imagePath;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get image path: {ex.Message}");
        }
    }

    public string? GetNormalImageName(string? imagePath)
    {
        if (imagePath != null)
            return imagePath.Remove(0, _targetDirectory.Length);
        return null;
    }
}