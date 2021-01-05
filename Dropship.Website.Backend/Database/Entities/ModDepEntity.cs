namespace Dropship.Website.Backend.Database.Entities
{
    public class ModDepEntity
    {
        public int Id { get; set; }
        public int ModBuildId { get; set; }
        public int DepModBuildId { get; set; }

        // Relationships.
        public ModBuildEntity ModBuild { get; set; }
        
        public ModBuildEntity DepModBuild { get; set; }
    }
}