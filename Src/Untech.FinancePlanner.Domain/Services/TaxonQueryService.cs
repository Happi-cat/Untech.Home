using System.Collections.Generic;
using System.Linq;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.ViewModels;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace Untech.FinancePlanner.Domain.Services
{
	public class TaxonQueryService :
		IQueryHandler<TaxonTreeQuery, TaxonTree>
	{
		private static readonly IReadOnlyList<Taxon> s_builtIns;
		private readonly IDataStorage<Taxon> _dataStorage;

		static TaxonQueryService()
		{
			s_builtIns = new List<Taxon>
			{
				new Taxon(BuiltInTaxonId.Root, BuiltInTaxonId.Root, "Root"),
				new Taxon(BuiltInTaxonId.Expense, BuiltInTaxonId.Root, "Expense"),
				new Taxon(BuiltInTaxonId.Saving, BuiltInTaxonId.Root, "Saving"),
				new Taxon(BuiltInTaxonId.Income, BuiltInTaxonId.Root, "Income"),
			};
		}

		public TaxonQueryService(IDataStorage<Taxon> dataStorage)
		{
			_dataStorage = dataStorage;
		}

		public TaxonTree Handle(TaxonTreeQuery request)
		{
			var builtInTaxon = s_builtIns.SingleOrDefault(n => n.Key == request.TaxonKey);
			if (builtInTaxon != null)
			{
				return new TaxonTree(builtInTaxon.Key, builtInTaxon.ParentKey, builtInTaxon.Name)
				{
					Elements = GetDescendants(builtInTaxon.Key, request.Deep)
				};
			}

			var taxon = _dataStorage.Find(n => n.Key == request.TaxonKey).Single();

			return new TaxonTree(taxon.Key, taxon.ParentKey, taxon.Name, taxon.Description)
			{
				Elements = GetDescendants(taxon.Key, request.Deep)
			};
		}

		private List<TaxonTree> GetDescendants(int parentTaxonKey, int deep)
		{
			if (deep == 0) return new List<TaxonTree>();

			deep = DecrementDeep(deep);

			var elements = GetElements(parentTaxonKey);

			if (deep != 0)
			{
				foreach (var taxon in elements)
				{
					taxon.Elements = GetDescendants(taxon.Key, deep);
				}
			}
			return elements;
		}

		private List<TaxonTree> GetElements(int parentTaxonKey)
		{
			if (parentTaxonKey == 0)
			{
				return s_builtIns
					.Where(n => n.ParentKey == 0 && n.Key != 0)
					.Select(n => new TaxonTree(n.Key, n.ParentKey, n.Name))
					.ToList();
			}

			return _dataStorage.Find(n => n.ParentKey == parentTaxonKey)
				.Select(n => new TaxonTree(n.Key, n.ParentKey, n.Name, n.Description))
				.ToList();
		}

		private int DecrementDeep(int deep) => (deep > 0) ? --deep : deep;
	}
}