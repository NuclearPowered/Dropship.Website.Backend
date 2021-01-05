namespace Dropship.Website.Backend.Database.Entities
{
    public class ServerEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IpAddress { get; set; }
        public ushort Port { get; set; }
        public int OwnerUserId { get; set; }
        public int StarCount { get; set; }
        public string ImageUrl { get; set; }

        // Navigation properties
        public UserEntity Owner { get; set; }
    }
}