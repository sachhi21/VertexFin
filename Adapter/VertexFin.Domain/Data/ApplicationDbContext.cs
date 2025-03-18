using Microsoft.EntityFrameworkCore;
using VertexFin.Domain.Models;

namespace VertexFin.Domain.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // ✅ DbSet Properties
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<KYC> KYCs { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
           .HasIndex(u => u.Username)
           .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            ConfigureUserRole(modelBuilder);
            ConfigureRolePermission(modelBuilder);
            ConfigureUserKYC(modelBuilder);
            ConfigureUserProfile(modelBuilder);
        }

        private static void ConfigureUserRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureRolePermission(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureUserKYC(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.KYC)
                .WithOne(k => k.User)
                .HasForeignKey<KYC>(k => k.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureUserProfile(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserProfile)
                .WithOne(up => up.User)
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
