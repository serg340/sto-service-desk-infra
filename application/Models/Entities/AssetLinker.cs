using STO_Desk_backend.Models.Enums;

namespace STO_Desk_backend.Models.Entities
{
    /// <summary>
    /// Represents a linker between entities and assets.
    /// Has polymorphic key EntityId which is carried by EntityType.
    /// </summary>
    public class AssetLinker
    {
        public int Id { get; set; }

        public int AssetId { get; set; }
        public Asset Asset { get; set; } = null!;

        public int EntityId { get; set; }
        public EntityType EntityType { get; set; }
    }
}
