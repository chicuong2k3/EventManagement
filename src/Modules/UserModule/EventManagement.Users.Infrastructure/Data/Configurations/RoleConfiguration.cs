using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Users.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Name);

            builder.Property(x => x.Name)
                .HasMaxLength(50);

            builder.HasMany<User>()
                .WithMany(x => x.Roles)
                .UsingEntity(joinEntity =>
                {
                    joinEntity.ToTable("user_roles");
                    joinEntity.Property("RolesName").HasColumnName("role_name");
                });

            builder.HasData(Role.Admin, Role.Member);
        }
    }
}
