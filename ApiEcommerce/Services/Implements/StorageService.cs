using ApiEcommerce.Entities;
using ApiEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Implements
{
    public class StorageService : IStorageService
    {
        readonly string separator = "/";

        private readonly IWebHostEnvironment _hostingEnvironment;

        public StorageService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            if (!Directory.Exists(ImageUploadDirectory))
                CreateFolder("");
        }

        public string WebRoot => _hostingEnvironment.WebRootPath.Replace("\\", "/");

        public string ContentRootPath => _hostingEnvironment.ContentRootPath.Replace("\\", "/");

        public string ImageUploadDirectory
        {
            get
            {
                var path = WebRoot ??
                           Path.Combine(ContentRootPath, "wwwroot");

                path = Path.Combine(path, "images");

                return path;
            }
        }


        public void CreateFolder(string path)
        {
            var dir = GetFullPath(path);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public string GetFileExtension(string fileName)
        {
            string[] parts = fileName.Split(".");
            string extension = parts[parts.Length - 1];
            if (extension.StartsWith("png", StringComparison.OrdinalIgnoreCase)
                || extension.StartsWith("jpeg", StringComparison.OrdinalIgnoreCase)
                || extension.StartsWith("jpg",
                    StringComparison.OrdinalIgnoreCase))
                return "." + extension;
            else
            {
                throw new Exception(
                    "For security reasons it is not allowed to upload files other than png or jpeg");
            }
        }

        public string GetFullPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return ImageUploadDirectory;
            else
                return Path.Combine(ImageUploadDirectory, path.Replace("/", separator));
        }

        public string GetRandomFileName()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
        }

        public string TrimFilePath(string path)
        {
            var p = path.Replace(WebRoot, "");
            if (p.StartsWith("\\")) p = p.Substring(1);
            return p;
        }

        public async Task<FileUpload> UploadFormFile(IFormFile file, string path = "")
        {
            VerifyPath(path);
            var fileName = GetRandomFileName() + GetFileExtension(file.FileName);
            var filePath = string.IsNullOrEmpty(path)
                ? Path.Combine(ImageUploadDirectory, fileName)
                : Path.Combine(ImageUploadDirectory, path + separator + fileName);

            filePath = filePath.Replace("\\", "/");

            using var fileStream = new FileStream(filePath, FileMode.Create);
            {
                await file.CopyToAsync(fileStream);

                return new FileUpload
                {
                    OriginalFileName = file.FileName,
                    FileName = fileName,
                    FilePath = TrimFilePath(filePath),
                    FileSize = file.Length,
                };
            }
        }

        public void VerifyPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var dir = Path.Combine(ImageUploadDirectory, path);

                if (!Directory.Exists(dir))
                {
                    CreateFolder(dir);
                }
            }
        }
    }
}
