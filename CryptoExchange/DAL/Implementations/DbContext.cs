using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Persons;
using Core.Models.Wallets;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Implementations
{
    public class AppDbContext : DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Coin> Coins { get; set; }
        public DbSet<DepositDeal> DepositDeals { get; set; }
        public DbSet<FuethersDeal> FuethersDeals { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PriceHistory> PriceHistories { get; set; }
        public DbSet<SeedPhrase> SeedPhrases { get; set; }
        public DbSet<Support> Supports{ get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletForMarket> WalletForMarkets { get; set; }

        IConfiguration appConnfig;

        public AppDbContext(IConfiguration configuration)
        {
            appConnfig = configuration;
        }

        public AppDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-U1OM050\\SQLEXPRESS01;Database=CryptoCurrencyExchange;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
