using Microsoft.AspNetCore.Http;

using System.IO;
using System.Threading.Tasks;

namespace BackendComfeco.ExtensionMethods
{
    public static class IFormFileExtensionMethods
    {
        public static async Task<byte[]> ConvertToByteArray(this IFormFile formFile)
        {

            using var memoryStream = await formFile.ConvertToMemoryStream();

           return memoryStream.ToArray();
        
        }


        public static async Task<MemoryStream> ConvertToMemoryStream(this IFormFile formFile)
        {

            using MemoryStream memoryStream = new MemoryStream();

            await formFile.CopyToAsync(memoryStream);

            return memoryStream;

        }
    }
}
