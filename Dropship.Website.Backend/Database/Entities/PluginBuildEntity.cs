using System.Collections.Generic;

namespace Dropship.Website.Backend.Database.Entities
{
    public class PluginBuildEntity
    {
        public int Id { get; set; }

        /// <summary>
        ///     Foreign key Id for the PluginEntity that the build is for.
        /// </summary>
        public int PluginId { get; set; }
        
        /// <summary>
        ///     Incremental version code per <see cref="PluginId" />.
        ///     Starts at 1, increments by 1 for every build for a <see cref="PluginId" />.
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
        ///     Download URL for the plugin build.
        /// </summary>
        public string DownloadUrl { get; set; }

        /// <summary>
        ///     Flag to set if a build was deleted/revoked.
        /// </summary>
        public bool Deleted { get; set; }

        // Relationships.
        public PluginEntity Plugin { get; set; }
    }
}