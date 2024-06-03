﻿// <auto-generated />
using System;
using DAL.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240520154815_supportUpdate")]
    partial class supportUpdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Models.DepositDeal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("AmountInUSDT")
                        .HasColumnType("float");

                    b.Property<DateTime>("CloseTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Coin")
                        .HasColumnType("int");

                    b.Property<double>("ExpectableIncome")
                        .HasColumnType("float");

                    b.Property<double>("MonthIncomeInPercents")
                        .HasColumnType("float");

                    b.Property<int>("PeriodInMonth")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeOfOpen")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("DepositDeals");
                });

            modelBuilder.Entity("Core.Models.FuethersDeal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<Guid>("CoinId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("EnterPrice")
                        .HasColumnType("float");

                    b.Property<int>("Leverage")
                        .HasColumnType("int");

                    b.Property<double>("MarginValue")
                        .HasColumnType("float");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<double>("StopLoss")
                        .HasColumnType("float");

                    b.Property<double>("TakeProfit")
                        .HasColumnType("float");

                    b.Property<int>("TypeOfDeal")
                        .HasColumnType("int");

                    b.Property<int>("TypeOfMargin")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("FuethersDeals");
                });

            modelBuilder.Entity("Core.Models.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Core.Models.Persons.AdminAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdOfAdmin")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("TypeOfAction")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("AdminAction");
                });

            modelBuilder.Entity("Core.Models.Persons.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<double>("BalanceForCopyTrading")
                        .HasColumnType("float");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FollowersIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<double>("Income")
                        .HasColumnType("float");

                    b.Property<bool>("IsAvailableForCopyTrade")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WalletId");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Core.Models.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Core.Models.Wallets.Coin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("Name")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<Guid?>("WalletForMarketId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WalletForMarketId");

                    b.HasIndex("WalletId");

                    b.ToTable("Coins");
                });

            modelBuilder.Entity("Core.Models.Wallets.PriceHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CoinId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CoinPrices")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CoinId");

                    b.ToTable("PriceHistories");
                });

            modelBuilder.Entity("Core.Models.Wallets.SeedPhrase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SeedPhraseValues")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SeedPhrases");
                });

            modelBuilder.Entity("Core.Models.Wallets.Wallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SeedPhraseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SeedPhraseId");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("Core.Models.Wallets.WalletForMarket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SeedPhraseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SeedPhraseId");

                    b.ToTable("WalletForMarkets");
                });

            modelBuilder.Entity("Core.Models.Persons.Support", b =>
                {
                    b.HasBaseType("Core.Models.Persons.User");

                    b.Property<int>("Experience")
                        .HasColumnType("int");

                    b.Property<int>("Salary")
                        .HasColumnType("int");

                    b.Property<Guid?>("TicketInProgressId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("TicketInProgressId");

                    b.HasDiscriminator().HasValue("Support");
                });

            modelBuilder.Entity("Core.Models.Persons.Admin", b =>
                {
                    b.HasBaseType("Core.Models.Persons.Support");

                    b.HasDiscriminator().HasValue("Admin");
                });

            modelBuilder.Entity("Core.Models.Message", b =>
                {
                    b.HasOne("Core.Models.Ticket", null)
                        .WithMany("ChatHistory")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Models.Persons.AdminAction", b =>
                {
                    b.HasOne("Core.Models.Persons.Admin", null)
                        .WithMany("HistoryOfActions")
                        .HasForeignKey("AdminId");
                });

            modelBuilder.Entity("Core.Models.Persons.User", b =>
                {
                    b.HasOne("Core.Models.Wallets.Wallet", "Wallet")
                        .WithMany()
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("Core.Models.Wallets.Coin", b =>
                {
                    b.HasOne("Core.Models.Wallets.WalletForMarket", null)
                        .WithMany("AmountOfCoins")
                        .HasForeignKey("WalletForMarketId");

                    b.HasOne("Core.Models.Wallets.Wallet", null)
                        .WithMany("AmountOfCoins")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Models.Wallets.PriceHistory", b =>
                {
                    b.HasOne("Core.Models.Wallets.Coin", "Coin")
                        .WithMany()
                        .HasForeignKey("CoinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coin");
                });

            modelBuilder.Entity("Core.Models.Wallets.Wallet", b =>
                {
                    b.HasOne("Core.Models.Wallets.SeedPhrase", "SeedPhrase")
                        .WithMany()
                        .HasForeignKey("SeedPhraseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SeedPhrase");
                });

            modelBuilder.Entity("Core.Models.Wallets.WalletForMarket", b =>
                {
                    b.HasOne("Core.Models.Wallets.SeedPhrase", "SeedPhrase")
                        .WithMany()
                        .HasForeignKey("SeedPhraseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SeedPhrase");
                });

            modelBuilder.Entity("Core.Models.Persons.Support", b =>
                {
                    b.HasOne("Core.Models.Ticket", "TicketInProgress")
                        .WithMany()
                        .HasForeignKey("TicketInProgressId");

                    b.Navigation("TicketInProgress");
                });

            modelBuilder.Entity("Core.Models.Ticket", b =>
                {
                    b.Navigation("ChatHistory");
                });

            modelBuilder.Entity("Core.Models.Wallets.Wallet", b =>
                {
                    b.Navigation("AmountOfCoins");
                });

            modelBuilder.Entity("Core.Models.Wallets.WalletForMarket", b =>
                {
                    b.Navigation("AmountOfCoins");
                });

            modelBuilder.Entity("Core.Models.Persons.Admin", b =>
                {
                    b.Navigation("HistoryOfActions");
                });
#pragma warning restore 612, 618
        }
    }
}
