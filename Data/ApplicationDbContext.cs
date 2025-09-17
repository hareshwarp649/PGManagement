using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PropertyManage.Data.Entities;

namespace PropertyManage.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Set key lengths explicitly to avoid exceeding 900 bytes

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(l => new { l.LoginProvider, l.ProviderKey });
                entity.Property(l => l.LoginProvider).HasMaxLength(450);
                entity.Property(l => l.ProviderKey).HasMaxLength(450);
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(r => new { r.UserId, r.RoleId });
                entity.Property(r => r.UserId).HasMaxLength(450);
                entity.Property(r => r.RoleId).HasMaxLength(450);
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
                entity.Property(t => t.LoginProvider).HasMaxLength(450);
                entity.Property(t => t.Name).HasMaxLength(450);
            });


            // One-to-Many Relationship: Employee -> EmployeeDocuments
            builder.Entity<EmployeeDocument>()
                .HasOne(ud => ud.Employee)
                .WithMany(u => u.Documents)
                .HasForeignKey(ud => ud.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.Entity<UserRole>()
                .Property(ur => ur.UserId)
                .HasColumnType("nvarchar(450)")
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


            builder.Entity<Employee>()
                .HasOne(e => e.Manager)        // One Employee has One Manager
                .WithMany(e => e.Subordinates) // One Manager has Many Employees
                .HasForeignKey(e => e.ManagerId) // Foreign Key
                .OnDelete(DeleteBehavior.Restrict); // Prevent Cascade Delete

            // Designation
            builder.Entity<Employee>()
                .HasOne(l => l.Role)
                .WithMany()
                .HasForeignKey(l => l.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public new DbSet<Role> Roles { get; set; }
        public new DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserDocument> UserDocuments { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
    }
}
