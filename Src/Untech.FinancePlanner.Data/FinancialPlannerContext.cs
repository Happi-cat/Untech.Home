using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Untech.FinancePlanner.Data.Cache;
using Untech.FinancePlanner.Domain.Models;

namespace Untech.FinancePlanner.Data
{
	public class FinancialPlannerContext : DbContext
	{
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
				.OwnsOne(e => e.Actual, mb => ConfigureMoneyColumns("Actual", mb))
				.OwnsOne(e => e.Forecasted, mb => ConfigureMoneyColumns("Forecasted", mb));

			modelBuilder.Entity<CacheEntry>()
				.HasKey(c => c.Key);
		}

		private static void ConfigureMoneyColumns(string prefix, ReferenceOwnershipBuilder<FinancialJournalEntry, Practices.Money> moneyBuilder)
		{
			moneyBuilder.Property(m => m.Amount).HasColumnName(prefix + "Amount");

			moneyBuilder.OwnsOne(m => m.Currency, mb => mb
				.Ignore(m => m.Name)
				.Property(m => m.Id).HasColumnName(prefix + "Currency"));
		}
	}
}
