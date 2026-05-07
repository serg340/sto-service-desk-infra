using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Configurations
{
    public class RoleTicketCategoryConfiguration : IEntityTypeConfiguration<RoleTicketCategory>
    {
        public void Configure(EntityTypeBuilder<RoleTicketCategory> builder)
        {
            builder.Property(tt => tt.Name)
                .IsRequired()
                .HasMaxLength(128);
        }
    }
}
