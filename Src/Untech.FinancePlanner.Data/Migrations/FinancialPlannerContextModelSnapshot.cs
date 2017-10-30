﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using Untech.FinancePlanner.Data;

namespace Untech.FinancePlanner.Data.Migrations
{
    [DbContext(typeof(FinancialPlannerContext))]
    partial class FinancialPlannerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("Untech.FinancePlanner.Data.Cache.CacheEntry", b =>
                {
                    b.Property<string>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Json");

                    b.HasKey("Key");

                    b.ToTable("CacheEntries");
                });

            modelBuilder.Entity("Untech.FinancePlanner.Domain.Models.FinancialJournalEntry", b =>
                {
                    b.Property<int>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Remarks");

                    b.Property<int>("TaxonKey");

                    b.Property<DateTime>("When");

                    b.HasKey("Key");

                    b.ToTable("FinancialJournalEntries");
                });

            modelBuilder.Entity("Untech.FinancePlanner.Domain.Models.Taxon", b =>
                {
                    b.Property<int>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("ParentKey");

                    b.HasKey("Key");

                    b.ToTable("Taxons");
                });

            modelBuilder.Entity("Untech.FinancePlanner.Domain.Models.FinancialJournalEntry", b =>
                {
                    b.OwnsOne("Untech.Practices.Money", "Actual", b1 =>
                        {
                            b1.Property<int?>("FinancialJournalEntryKey");

                            b1.Property<double>("Amount")
                                .HasColumnName("ActualAmount");

                            b1.ToTable("FinancialJournalEntries");

                            b1.HasOne("Untech.FinancePlanner.Domain.Models.FinancialJournalEntry")
                                .WithOne("Actual")
                                .HasForeignKey("Untech.Practices.Money", "FinancialJournalEntryKey")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.OwnsOne("Untech.Practices.Currency", "Currency", b2 =>
                                {
                                    b2.Property<int?>("MoneyFinancialJournalEntryKey");

                                    b2.Property<string>("Id")
                                        .HasColumnName("ActualCurrency");

                                    b2.ToTable("FinancialJournalEntries");

                                    b2.HasOne("Untech.Practices.Money")
                                        .WithOne("Currency")
                                        .HasForeignKey("Untech.Practices.Currency", "MoneyFinancialJournalEntryKey")
                                        .OnDelete(DeleteBehavior.Cascade);
                                });
                        });

                    b.OwnsOne("Untech.Practices.Money", "Forecasted", b1 =>
                        {
                            b1.Property<int>("FinancialJournalEntryKey");

                            b1.Property<double>("Amount")
                                .HasColumnName("ForecastedAmount");

                            b1.ToTable("FinancialJournalEntries");

                            b1.HasOne("Untech.FinancePlanner.Domain.Models.FinancialJournalEntry")
                                .WithOne("Forecasted")
                                .HasForeignKey("Untech.Practices.Money", "FinancialJournalEntryKey")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.OwnsOne("Untech.Practices.Currency", "Currency", b2 =>
                                {
                                    b2.Property<int>("MoneyFinancialJournalEntryKey");

                                    b2.Property<string>("Id")
                                        .HasColumnName("ForecastedCurrency");

                                    b2.ToTable("FinancialJournalEntries");

                                    b2.HasOne("Untech.Practices.Money")
                                        .WithOne("Currency")
                                        .HasForeignKey("Untech.Practices.Currency", "MoneyFinancialJournalEntryKey")
                                        .OnDelete(DeleteBehavior.Cascade);
                                });
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
