using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PropertyManage.Data.Entities;
using PropertyManage.Data.MasterEntities;

namespace PropertyManage.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
    IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Set key lengths explicitly to avoid exceeding 900 bytes

            builder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.HasKey(l => new { l.LoginProvider, l.ProviderKey });
                entity.Property(l => l.LoginProvider).HasMaxLength(450);
                entity.Property(l => l.ProviderKey).HasMaxLength(450);
            });

            builder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.HasKey(r => new { r.UserId, r.RoleId });
                entity.Property(r => r.UserId).HasMaxLength(450);
                entity.Property(r => r.RoleId).HasMaxLength(450);
            });

            builder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
                entity.Property(t => t.LoginProvider).HasMaxLength(450);
                entity.Property(t => t.Name).HasMaxLength(450);
            });




            builder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.Entity<UserRole>()
                .Property(ur => ur.UserId)
                .HasColumnType("uniqueidentifier")
                .HasMaxLength(450);

            builder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            builder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .HasPrincipalKey(u => u.Id);


            builder.Entity<RolePermission>()
                .HasKey(ur => new { ur.RoleId, ur.PermissionId });

            builder.Entity<RolePermission>()
                .HasOne(ur => ur.Role)
                .WithMany(u => u.RolePermissions)
                .HasForeignKey(ur => ur.RoleId);

            builder.Entity<RolePermission>()
                .HasOne(ur => ur.Permission)
                .WithMany(u => u.RolePermissions)
                .HasForeignKey(ur => ur.PermissionId);

            // Permission unique name
            builder.Entity<Permission>(b =>
            {
                b.HasKey(p => p.Id);
                b.HasIndex(p => p.Name).IsUnique();
                b.Property(p => p.Name).HasMaxLength(200).IsRequired();
            });

            // Refresh token
            builder.Entity<ApplicationUserRefreshToken>(b =>
            {
                b.HasKey(t => t.Id);
                b.HasIndex(t => t.Token).IsUnique();
                b.HasOne(t => t.User).WithMany(u => u.RefreshTokens).HasForeignKey(t => t.UserId);
            });



          
        }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        //public new DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public new DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ApplicationUserRefreshToken> RefreshTokens { get; set; } = null!;

        public DbSet<UserDocument> UserDocuments { get; set; }
 

        // Add other DbSets for your entities here

        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<District> Districts { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<PaymentMode> PaymentModes { get; set; }
        public DbSet<RentPlan> RentPlans { get; set; }
        public DbSet<UtilityType> UtilityTypes { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }

        public DbSet<Propertiy> Properties { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Tenant> Tenants { get; set; }



    }
}
