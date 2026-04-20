using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamingStore.DAL.Config
{
    public class IdentityRoleConfig : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole { Id = "11111111-1111-1111-1111-111111111111", Name = "User", NormalizedName = "USER" },
                new IdentityRole { Id = "22222222-2222-2222-2222-222222222222", Name = "Editor", NormalizedName = "EDITOR" },
                new IdentityRole { Id = "33333333-3333-3333-3333-333333333333", Name = "Admin", NormalizedName = "ADMIN" }
            );
        }
    }
}
