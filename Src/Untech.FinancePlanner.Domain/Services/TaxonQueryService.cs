using System.Collections.Generic;
using System.Linq;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.Storage;
using Untech.FinancePlanner.Domain.ViewModels;
using Untech.Practices.CQRS.Handlers;

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
			var builtInTaxon = s_builtIns.SingleOrDefault(n => n.Id == request.TaxonId);
			if (builtInTaxon != null)
			{
				return new TaxonTree(builtInTaxon.Id, builtInTaxon.ParentId, builtInTaxon.Name)
				{
					Elements = GetDescendants(builtInTaxon.Id, request.Deep)
				};
			}

			var taxon = _dataStorage.Find(n => n.Id == request.TaxonId).Single();

			return new TaxonTree(taxon.Id, taxon.ParentId, taxon.Name, taxon.Description)
			{
				Elements = GetDescendants(taxon.Id, request.Deep)
			};
		}

		private List<TaxonTree> GetDescendants(int parentTaxonId, int deep)
		{
			if (deep == 0) return new List<TaxonTree>();

			deep = DecrementDeep(deep);

			var elements = GetElements(parentTaxonId);

			if (deep != 0)
			{
				foreach (var taxon in elements)
				{
					taxon.Elements = GetDescendants(taxon.Id, deep);
				}
			}
			return elements;
		}

		private List<TaxonTree> GetElements(int parentTaxonId)
		{
			if (parentTaxonId == 0)
			{
				return s_builtIns
					.Where(n => n.ParentId == 0 && n.Id != 0)
					.Select(n => new TaxonTree(n.Id, n.ParentId, n.Name))
					.ToList();
			}

			return _dataStorage.Find(n => n.ParentId == parentTaxonId)
				.Select(n => new TaxonTree(n.Id, n.ParentId, n.Name, n.Description))
				.ToList();
		}

		private int DecrementDeep(int deep) => (deep > 0) ? --deep : deep;
	}
}