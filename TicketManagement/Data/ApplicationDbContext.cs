using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Entities;

namespace TicketManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed promo codes
            modelBuilder.Entity<PromoCode>().HasData(
                new PromoCode
                {
                    Id = 1,
                    Code = "EARLYBIRD",
                    Discount = 10.00m,
                    IsActive = true
                },
                new PromoCode
                {
                    Id = 2,
                    Code = "WELCOME2024",
                    Discount = 5.00m,
                    IsActive = true
                },
                new PromoCode
                {
                    Id = 3,
                    Code = "SUMMERFUN",
                    Discount = 15.00m,
                    IsActive = true
                }
            );

            // Seed ticket types
            modelBuilder.Entity<TicketType>().HasData(
               new TicketType
               {
                   Id = 1,
                   Name = "Male - Early Bird",
                   Price = 15m,
                   AvailableQuantity = 100
               },
               new TicketType
               {
                   Id = 2,
                   Name = "Female - Early Bird",
                   Price = 25m,
                   AvailableQuantity = 10
               },
               new TicketType
               {
                   Id = 3,
                   Name = "Male - Standard Release",
                   Price = 15.00m,
                   AvailableQuantity = 0
               },
               new TicketType
               {
                   Id = 4,
                   Name = "Female - Standard Release",
                   Price = 25.00m,
                   AvailableQuantity = 0
               }
           );
        }
    }
}