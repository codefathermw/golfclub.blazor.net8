using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolfClub.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.DAL.Context
{
    public class AppDbContext : DbContext
    {
            public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
        public DbSet<UserAccount> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Fitting> FittingRequests { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // table: users
            modelBuilder.Entity<UserAccount>().ToTable("users");
            modelBuilder.Entity<UserAccount>().HasKey(u => u.UserId).HasName("pk_users");
            modelBuilder.Entity<UserAccount>()
                .HasMany(u => u.FittingRequests)
                .WithOne(fr => fr.User)
                .HasForeignKey(fr => fr.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserAccount>()
                .HasOne(u => u.UserProfile)
                .WithOne(f => f.User)
                .HasForeignKey<UserProfile>(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserAccount>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);
            // data
            string passwordHash = "hashedpassword";
            modelBuilder.Entity<UserAccount>().HasData(
                new UserAccount { UserId = 1, UserName = "admin", PasswordHash = passwordHash }
            );

            // table: roles
            modelBuilder.Entity<Role>().ToTable("roles");
            modelBuilder.Entity<Role>().HasKey(r => r.RoleId).HasName("pk_roles");
            // data
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "UserAccount" }
            );

            // table: user_roles
            modelBuilder.Entity<UserRole>().ToTable("user_roles");
            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId }).HasName("pk_user_roles");
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
            // data
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { UserId = 1, RoleId = 1 }
            );

            // table: fitting
            modelBuilder.Entity<Fitting>().ToTable("fitting");
            modelBuilder.Entity<Fitting>().HasKey(fr => fr.FittingId).HasName("pk_fitting");
            modelBuilder.Entity<Fitting>()
                .HasOne(fr => fr.User)
                .WithMany(u => u.FittingRequests)
                .HasForeignKey(fr => fr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // table: user_profiles
            modelBuilder.Entity<UserProfile>().ToTable("user_profiles");
            modelBuilder.Entity<UserProfile>().HasKey(cp => cp.UserId).HasName("pk_user_profiles");
            modelBuilder.Entity<UserProfile>().HasData(
                new UserProfile { UserId = 1, FirstName = "Andre", LastName = "Khonje", Email = "admin@example.com", Address = "123 Admin St", Phone = "555-1234", GolfClubSize = "Standard" }
            );
        }
    }
}
