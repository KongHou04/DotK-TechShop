using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace DotK_TechShop.Services;

public class FileUploader(string targetDirectory)
{
    private readonly string _targetDirectory = targetDirectory;
    public string Upload(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is not provided or empty.");
            }

            if (!Directory.Exists(_targetDirectory))
            {
                Directory.CreateDirectory(_targetDirectory);
            }

            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(_targetDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }
        catch (Exception ex)
        {
            throw new Exception($"File upload failed: {ex.Message}");
        }
    }

    public void Delete(string fileName)
    {
        try
        {
            string filePath = Path.Combine(_targetDirectory, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"File deletion failed: {ex.Message}");
        }
    }

}
