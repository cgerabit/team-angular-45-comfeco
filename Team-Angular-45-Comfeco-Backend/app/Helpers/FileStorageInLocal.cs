using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackendComfeco.Helpers
{
    public class FileStorageInLocal : IFileStorage
    {
        private readonly IWebHostEnvironment env;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private string _CurrentUrl;
        public string CurrentUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_CurrentUrl))
                {
                    using var scope = serviceScopeFactory.CreateScope();

                    var contextAccesor = scope.ServiceProvider.GetService<IHttpContextAccessor>();

                    _CurrentUrl = $"{contextAccesor.HttpContext.Request.Scheme}://{contextAccesor.HttpContext.Request.Host}";
                }

                return _CurrentUrl;
            }
        }
        public FileStorageInLocal(IWebHostEnvironment env,
          IServiceScopeFactory serviceScopeFactory)
        {
            this.env = env;
            this.serviceScopeFactory = serviceScopeFactory;
        }
        public async Task<string> EditFile(byte[] content, string fileExtension, string container, string currentRoute, string contentType)
        {
            string fileName = Path.GetFileName(currentRoute);
            await RemoveFile(currentRoute, container);
            return await SaveFile(content, fileExtension, container, contentType, fileName);
        }

        public Task RemoveFile(string route, string container)
        {
            if (route != null)
            {
                var fileName = Path.GetFileName(route);
                string filePath = Path.Combine(env.WebRootPath, container, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            return Task.CompletedTask;
        }

        public async Task<string> SaveFile(byte[] content, string fileExtension, string container, string contentType, string FileName)
        {
            string fullFileName = $"{FileName}{(fileExtension.StartsWith(".") ? fileExtension : $".{fileExtension}")}";

            string folder = Path.Combine(env.WebRootPath, container);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);

            }

            string fullPath = Path.Combine(folder, fullFileName);

            await File.WriteAllBytesAsync(fullPath, content);

            string dbUrl = Path.Combine(CurrentUrl, container, fullFileName).Replace("\\", "/");


            return dbUrl;
        }
    }
}
