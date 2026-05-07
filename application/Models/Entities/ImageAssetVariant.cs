using STO_Desk_backend.Models.Enums;

namespace STO_Desk_backend.Models.Entities
{
    /// <summary>
    /// Represents a variant of an image-type media asset. <br></br>
    /// Uses ImageFormatVariant enum for variant types. <br></br>
    /// Has connection with ImageAsset.
    /// </summary>
    public class ImageAssetVariant
    {
        public int Id { get; set; }

        public int ImageAssetId { get; set; }
        public ImageAsset Image { get; set; } = null!;

        public string ObjectName { get; set; } = string.Empty;

        public ImageFormatVariant SizeType { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
