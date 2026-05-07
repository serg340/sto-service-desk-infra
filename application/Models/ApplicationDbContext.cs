using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models
{
    /// <summary>
    /// DB.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<RoleTicket> RoleTickets => Set<RoleTicket>();

        public DbSet<Sto> Stos => Set<Sto>();

        public DbSet<Region> Regions => Set<Region>();

        public DbSet<TicketCategory> TicketCategories => Set<TicketCategory>();
        public DbSet<TicketTheme> TicketThemes => Set<TicketTheme>();
        public DbSet<RoleTicketCategory> RoleTicketCategories => Set<RoleTicketCategory>();
        public DbSet<RoleTicketTheme> RoleTicketThemes => Set<RoleTicketTheme>();

        public DbSet<Asset> Assets => Set<Asset>();
        public DbSet<ImageAsset> ImageAssets => Set<ImageAsset>();
        public DbSet<ImageAssetVariant> ImageAssetVariants => Set<ImageAssetVariant>();
        public DbSet<DocumentAsset> DocumentAssets => Set<DocumentAsset>();
        public DbSet<AssetLinker> AssetLinker => Set<AssetLinker>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

    }
}
