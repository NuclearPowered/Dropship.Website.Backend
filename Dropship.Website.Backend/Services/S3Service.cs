using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Dropship.Website.Backend.Models.Configuration;
using Microsoft.Extensions.Options;

namespace Dropship.Website.Backend.Services
{
    public class S3Service
    {
        private readonly SpacesConfig _spacesConfig;
        internal TransferUtility TransferUtility { get; }

        public S3Service(IOptions<SpacesConfig> spacesConfig)
        {
            _spacesConfig = spacesConfig.Value;
            
            var credentials = new BasicAWSCredentials(_spacesConfig.AccessKey, _spacesConfig.SecretKey);
            var config = new AmazonS3Config
            {
                ServiceURL = _spacesConfig.ServiceUrl 
            };
            var client = new AmazonS3Client(credentials, config); 
            TransferUtility = new TransferUtility(client);
        }
    }
}