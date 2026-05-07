namespace STO_Desk_backend.Models.Entities
{
    /// <summary>
    /// Represents a document asset. <br></br>
    /// Has connection with Asset.
    /// </summary>
    public class DocumentAsset
    {
        public int Id { get; set; }

        public int AssetId { get; set; }
        public Asset Asset { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
