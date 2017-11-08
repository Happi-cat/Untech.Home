using LinqToDB.Mapping;
using Untech.FinancePlanner.Domain.Models;

namespace Untech.FinancePlanner.Data
{
	[Table("Taxons")]
	public class TaxonDto
	{
		[Column, PrimaryKey, Identity]
		public int Key { get; set; }

		[Column, NotNull]
		public int ParentKey { get; set; }

		[Column, NotNull]
		public string Name { get; set; }

		[Column]
		public string Description { get; set; }

		public static Taxon Convert(TaxonDto dto)
		{
			return new Taxon(dto.Key, dto.ParentKey, dto.Name, dto.Description);
		}

		public static TaxonDto Convert(Taxon entry)
		{
			return new TaxonDto
			{
				Key = entry.Key,
				ParentKey = entry.ParentKey,
				Name = entry.Name,
				Description = entry.Description
			};
		}
	}
}