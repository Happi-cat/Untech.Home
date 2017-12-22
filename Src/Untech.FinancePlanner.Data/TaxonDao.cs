using LinqToDB.Mapping;
using Untech.FinancePlanner.Domain.Models;

namespace Untech.FinancePlanner.Data
{
	[Table("Taxons")]
	public class TaxonDao
	{
		public TaxonDao()
		{

		}

		public TaxonDao(Taxon entry)
		{
			Key = entry.Key;
			ParentKey = entry.ParentKey;
			Name = entry.Name;
			Description = entry.Description;
		}

		[Column, PrimaryKey, Identity]
		public int Key { get; set; }

		[Column, NotNull]
		public int ParentKey { get; set; }

		[Column, NotNull]
		public string Name { get; set; }

		[Column]
		public string Description { get; set; }

		public static Taxon ToEntity(TaxonDao dao)
		{
			return new Taxon(dao.Key, dao.ParentKey, dao.Name, dao.Description);
		}
	}
}