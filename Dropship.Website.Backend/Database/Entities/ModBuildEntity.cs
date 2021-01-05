using System.Collections.Generic;

namespace Dropship.Website.Backend.Database.Entities
{
    public class ModBuildEntity
    {
        public int Id { get; set; }

        /// <summary>
        ///     Foreign key Id for the ModEntity that the build is for.
        /// </summary>
        public int ModId { get; set; }
        
        /// <summary>
        ///     Incremental version code per <see cref="ModId" />.
        ///     Starts at 1, increments by 1 for every build for a <see cref="ModId" />.
        /// </summary>
        public int VersionCode { get; set; }

        /// <summary>
        ///     Human readable version string in semver.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        ///     FileName, purely for UI purposes.
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        ///     Download URL for the mod build.
        /// </summary>
        public string DownloadUrl { get; set; }

        /// <summary>
        ///     Flag to set if a build was deleted/revoked.
        /// </summary>
        public bool Deleted { get; set; }

        // Relationships.
        public ModEntity Mod { get; set; }
        public List<ModDepEntity> Dependencies { get; set; }
        public List<ModDepEntity> Dependents { get; set; }
    }
}