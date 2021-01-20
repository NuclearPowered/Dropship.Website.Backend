namespace Dropship.Website.Backend.Database.DataTypes
{
    public enum ServerDistro
    {
        /// <summary>
        ///     Used for error checking, should not be stored.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     Plugin is server-side Impostor plugin.
        /// </summary>
        Impostor = 1,

        /// <summary>
        ///     Plugin is server-side NodePolus plugin.
        /// </summary>
        NodePolus = 2,
    }
}