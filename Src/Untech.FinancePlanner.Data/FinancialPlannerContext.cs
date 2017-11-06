using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Untech.FinancePlanner.Data.Cache;
using Untech.FinancePlanner.Domain.Models;

namespace Untech.FinancePlanner.Data
{
	public class FinancialPlannerContext : DbContext
	{
		public FinancialPlannerContext()
		{
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}

		public DbSet<Taxon> Taxons { get; set; }

		public DbSet<FinancialJournalEntry> FinancialJournalEntries { get; set; }

		public DbSet<CacheEntry> CacheEntries { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=financial_planner.db");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<FinancialJournalEntry>()
				.OwnsOne(e => e.Forecasted, mb => ConfigureMoneyColumns("Forecasted", mb))
				.OwnsOne(e => e.Actual, mb => ConfigureMoneyColumns("Actual", mb));

			modelBuilder.Entity<FinancialJournalEntry>().HasKey(e => e.Key);
			modelBuilder.Entity<Taxon>().HasKey(e => e.Key);
			modelBuilder.Entity<CacheEntry>().HasKey(e => e.Key);
		}

		private static void ConfigureMoneyColumns(string prefix, ReferenceOwnershipBuilder<FinancialJournalEntry, Practices.Money> builder)
		{
			builder.Property<int>("FinancialJournalEntryKey");

			builder.Property(m => m.Amount).HasColumnName(prefix + "Amount");

			builder.OwnsOne(m => m.Currency, mb =>
			{
				mb.Property<int>("MoneyFinancialJournalEntryKey");

				mb.Ignore(m => m.Name);
				mb.Property(m => m.Id).HasColumnName(prefix + "Currency");
			});
		}
	}
}
