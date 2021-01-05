using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Dropship.Website.Backend.Models.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Dropship.Website.Backend.Services
{
    public class ImageUploadService
    {
        private readonly SpacesConfig _spacesConfig;
        private readonly S3Service _s3;

        public ImageUploadService(IOptions<SpacesConfig> config, S3Service s3)
        {
            _spacesConfig = config.Value;
            _s3 = s3;
        }
        
        public async Task<string> UploadImage(IFormFile file)
        {
            await using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            if (stream.Length > 8388608) // 8 MB
            {
                return null;
            }

            var fileName = Guid.NewGuid() + ".png";
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = _spacesConfig.BucketName,
                CannedACL = S3CannedACL.PublicRead
            };
            await _s3.TransferUtility.UploadAsync(uploadRequest);
            return $"https://{_spacesConfig.BucketName}.{_spacesConfig.ServiceUrl.Replace("https://", "")}/{fileName}";
        }
    }
}