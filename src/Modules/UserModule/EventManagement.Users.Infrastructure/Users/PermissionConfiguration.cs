using EventManagement.Users.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Users.Infrastructure.Users
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(x => x.Code);

            builder.Property(x => x.Code)
                .HasMaxLength(100);

            builder.HasData(Permission.GetUser,
            Permission.ModifyUser,
            Permission.GetEvents,
            Permission.SearchEvents,
            Permission.ModifyEvents,
            Permission.GetTicketTypes,
            Permission.ModifyTicketTypes,
            Permission.GetCategories,
            Permission.ModifyCategories,
            Permission.GetCart,
            Permission.AddToCart,
            Permission.RemoveFromCart,
            Permission.GetOrders,
            Permission.CreateOrder,
            Permission.GetTickets,
            Permission.CheckInTicket,
            Permission.GetEventStatistics);

            builder.HasMany<Role>()
               .WithMany()
               .UsingEntity(joinEntity =>
               {
                   joinEntity.ToTable("role_permissions");

                   joinEntity.HasData(CreateRolePermission(Role.Member, Permission.GetUser),
                    CreateRolePermission(Role.Member, Permission.ModifyUser),
                    CreateRolePermission(Role.Member, Permission.SearchEvents),
                    CreateRolePermission(Role.Member, Permission.GetTicketTypes),
                    CreateRolePermission(Role.Member, Permission.GetCart),
                    CreateRolePermission(Role.Member, Permission.AddToCart),
                    CreateRolePermission(Role.Member, Permission.RemoveFromCart),
                    CreateRolePermission(Role.Member, Permission.GetOrders),
                    CreateRolePermission(Role.Member, Permission.CreateOrder),
                    CreateRolePermission(Role.Member, Permission.GetTickets),
                    CreateRolePermission(Role.Member, Permission.CheckInTicket),

                    CreateRolePermission(Role.Admin, Permission.GetUser),
                    CreateRolePermission(Role.Admin, Permission.ModifyUser),
                    CreateRolePermission(Role.Admin, Permission.GetEvents),
                    CreateRolePermission(Role.Admin, Permission.SearchEvents),
                    CreateRolePermission(Role.Admin, Permission.ModifyEvents),
                    CreateRolePermission(Role.Admin, Permission.GetTicketTypes),
                    CreateRolePermission(Role.Admin, Permission.ModifyTicketTypes),
                    CreateRolePermission(Role.Admin, Permission.GetCategories),
                    CreateRolePermission(Role.Admin, Permission.ModifyCategories),
                    CreateRolePermission(Role.Admin, Permission.GetCart),
                    CreateRolePermission(Role.Admin, Permission.AddToCart),
                    CreateRolePermission(Role.Admin, Permission.RemoveFromCart),
                    CreateRolePermission(Role.Admin, Permission.GetOrders),
                    CreateRolePermission(Role.Admin, Permission.CreateOrder),
                    CreateRolePermission(Role.Admin, Permission.GetTickets),
                    CreateRolePermission(Role.Admin, Permission.CheckInTicket),
                    CreateRolePermission(Role.Admin, Permission.GetEventStatistics));
               });
        }

        private static object CreateRolePermission(Role role, Permission permission)
        {
            return new
            {
                RoleName = role.Name,
                PermissionCode = permission.Code
            };
        }
    }
}
