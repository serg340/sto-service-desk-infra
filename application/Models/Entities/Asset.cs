using STO_Desk_backend.Models.Enums;

namespace STO_Desk_backend.Models.Entities
{
    /// <summary>
    /// Represents an asset that has actual asset's path to bucket where it is stored. <br></br>
    /// Uses ContentType enum to show what type of media asset it stores.
    /// </summary>
    public class Asset
    {
        public int Id { get; set; }

        public string ObjectName { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public ContentType ContentType { get; set; }
        public long Size { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
