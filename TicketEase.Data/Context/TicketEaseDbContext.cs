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
                MaintenanceMode = false,
            });

            // Admin user ekle
            var password = "Admin123.";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

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

        // Soft delete & UpdatedAt otomasyonu
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Deleted)
                {
                    // Soft delete uygula
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.UpdatedAt = DateTime.Now;

                    // Eğer Ticket soft delete ise -> TicketOrder soft delete
                    if (entry.Entity is TicketEntity ticket)
                    {
                        var ticketOrders = Set<TicketOrderEntity>()
                            .IgnoreQueryFilters()
                            .Where(to => to.TicketId == ticket.Id);

                        foreach (var to in ticketOrders)
                        {
                            to.IsDeleted = true;
                            to.UpdatedAt = DateTime.Now;
                        }
                    }

                    // Eğer Order soft delete ise -> TicketOrder soft delete
                    if (entry.Entity is OrderEntity order)
                    {
                        var ticketOrders = Set<TicketOrderEntity>()
                            .IgnoreQueryFilters()
                            .Where(to => to.OrderId == order.Id);

                        foreach (var to in ticketOrders)
                        {
                            to.IsDeleted = true;
                            to.UpdatedAt = DateTime.Now;
                        }
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.Now;

                    // 🔹 Eğer IsDeleted true olduysa propagation yap
                    if (entry.Entity is OrderEntity order && order.IsDeleted)
                    {
                        var ticketOrders = Set<TicketOrderEntity>()
                            .IgnoreQueryFilters()
                            .Where(to => to.OrderId == order.Id);

                        foreach (var to in ticketOrders)
                        {
                            to.IsDeleted = true;
                            to.UpdatedAt = DateTime.Now;
                        }
                    }

                    if (entry.Entity is TicketEntity ticket && ticket.IsDeleted)
                    {
                        var ticketOrders = Set<TicketOrderEntity>()
                            .IgnoreQueryFilters()
                            .Where(to => to.TicketId == ticket.Id);

                        foreach (var to in ticketOrders)
                        {
                            to.IsDeleted = true;
                            to.UpdatedAt = DateTime.Now;
                        }
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
