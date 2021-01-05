namespace Dropship.Website.Backend.Models.Configuration
{
    public class SpacesConfig
    {
        public const string Section = "S3API";

        public string AccessKey { get; set; } = "";

        public string SecretKey { get; set; } = "";

        public string ServiceUrl { get; set; } = "";

        public string BucketName { get; set; } = "";
    }
}