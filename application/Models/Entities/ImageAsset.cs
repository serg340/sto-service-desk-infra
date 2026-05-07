namespace STO_Desk_backend.Models.Entities
{
    public class ImageAsset
    {
        /// <summary>
        /// Represents an image. Has variants (ImageAssetVariant).
        /// Has connection with Asset.
        /// </summary>
        public int Id { get; set; }

        public int AssetId { get; set; }
        public Asset Asset { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Alt { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
