using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CarVilla.Helpers
{
    public static class FileManager
    {
        public static bool CheckImage(this IFormFile file)
        {
            return file.ContentType.Contains("image/") && file.Length / 1024 / 1024 <= 3;
        }
        public static string Upload(this IFormFile file, string web, string folderPath)
        {
            var uploadPath = web + folderPath;
            if (!Directory.Exists(uploadPath))
            {
                Directory.GetCreationTime(uploadPath);
            }
            string fileName = Guid.NewGuid().ToString() + file.FileName;
            string filePath = web + folderPath + fileName;
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }
    }
}
