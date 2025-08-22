using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketEase.Data.Entities;
using BCrypt.Net;
using TicketEase.Data.Enums;


namespace TicketEase.Data.Context
{
    public class TicketEaseDbContext : DbContext
    {

        public TicketEaseDbContext(DbContextOptions<TicketEaseDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API configuration’lar
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new TicketOrderConfiguration());
            modelBuilder.ApplyConfiguration(new TicketConfiguration());
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new VenueConfiguration());

            modelBuilder.Entity<SettingEntity>().HasData(new SettingEntity
            {
                Id = 1,
                CreatedAt = DateTime.Now,
                MaintenanceMode = false, // Default value for maintenance mode
            });

            //Adding admin user

            var password = "Admin123."; // admin password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password); // Hash the password

            modelBuilder.Entity<UserEntity>().HasData(new UserEntity
            {
                Id = 1, 
                Email = "admin@email.com",
                PasswordHash = passwordHash, 
                FirstName = "Selcan",
                LastName = "Yalçınkaya",
                BirthDate = new DateTime(1998, 2, 3),
                UserType = UserType.Admin,
                CreatedAt = DateTime.Now
            });

        }


        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<OrderEntity> Orders => Set<OrderEntity>();
        public DbSet<TicketOrderEntity> TicketOrders => Set<TicketOrderEntity>();
        public DbSet<TicketEntity> Tickets => Set<TicketEntity>();
        public DbSet<EventEntity> Events => Set<EventEntity>();
        public DbSet<PaymentEntity> Payments => Set<PaymentEntity>();
        public DbSet<VenueEntity> Venues => Set<VenueEntity>();

        public DbSet<SettingEntity> Settings => Set<SettingEntity>();

    }
}
