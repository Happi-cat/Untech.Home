using System.Dynamic;
using Untech.FinancePlanner.Domain.Models;
using Untech.Practices.ObjectMapping;

namespace Untech.FinancePlanner.Data
{
	internal class DaoMapper :
		IMap<FinancialJournalEntry, FinancialJournalEntryDao>,
		IMap<FinancialJournalEntryDao, FinancialJournalEntry>,
		IMap<Taxon, TaxonDao>,
		IMap<TaxonDao, Taxon>
	{
		public static readonly DaoMapper Instance = new DaoMapper();

		private DaoMapper()
		{
		}

		public FinancialJournalEntryDao Map(FinancialJournalEntry input)
		{
			return new FinancialJournalEntryDao(input);
		}

		public FinancialJournalEntry Map(FinancialJournalEntryDao input)
		{
			return FinancialJournalEntryDao.ToEntity(input);
		}

		public TaxonDao Map(Taxon input)
		{
			return new TaxonDao(input);
		}

		public Taxon Map(TaxonDao input)
		{
			return TaxonDao.ToEntity(input);
		}
	}
}