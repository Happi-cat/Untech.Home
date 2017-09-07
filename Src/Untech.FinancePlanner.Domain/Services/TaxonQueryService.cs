using System.Collections.Generic;
using System.Linq;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.ViewModels;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.Repos.Queryable;

namespace Untech.FinancePlanner.Domain.Services
{
	public class TaxonQueryService :
		IQueryHandler<TaxonTreeQuery, TaxonTree>
	{
		private readonly IReadOnlyRepository<Taxon> _repo;

		public TaxonQueryService(IReadOnlyRepository<Taxon> repo)
		{
			this._repo = repo;
		}

		public TaxonTree Handle(TaxonTreeQuery request)
		{
			if (request.TaxonId == 0)
			{
				return new TaxonTree(BuiltInTaxonId.Root, BuiltInTaxonId.Root, "Root")
				{
					Elements = GetDescendants(0, request.Deep)
				};
			}

			var taxon = _repo.GetAll().Single(n => n.Id == request.TaxonId);

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
				return new List<TaxonTree>
				{
					new TaxonTree(BuiltInTaxonId.Expense, BuiltInTaxonId.Root, "Expense"),
					new TaxonTree(BuiltInTaxonId.Saving, BuiltInTaxonId.Root, "Saving"),
					new TaxonTree(BuiltInTaxonId.Income, BuiltInTaxonId.Root, "Income"),
				};
			}

			return _repo.GetAll()
				.Where(n => n.ParentId == parentTaxonId)
				.Select(n => new TaxonTree(n.Id, n.ParentId, n.Name, n.Description))
				.ToList();
		}

		private int DecrementDeep(int deep) => (deep > 0) ? --deep : deep;
	}
}