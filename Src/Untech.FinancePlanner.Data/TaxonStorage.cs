using System;
using System.Collections.Generic;
using System.Linq;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.Home.Data;
using Untech.Practices.CQRS.Handlers;

namespace Untech.FinancePlanner.Data
{
	public class TaxonStorage : GenericDataStorage<Taxon, TaxonDao>,
		IQueryHandler<TaxonElementsQuery, IEnumerable<Taxon>>
	{
		private static readonly IReadOnlyList<Taxon> s_builtIns;

		static TaxonStorage()
		{
			s_builtIns = new List<Taxon>
			{
				new Taxon(BuiltInTaxonId.Root, BuiltInTaxonId.Root, "Root"),
				new Taxon(BuiltInTaxonId.Expense, BuiltInTaxonId.Root, "Expense"),
				new Taxon(BuiltInTaxonId.Saving, BuiltInTaxonId.Root, "Saving"),
				new Taxon(BuiltInTaxonId.Income, BuiltInTaxonId.Root, "Income"),
			};
		}

		public TaxonStorage(Func<FinancialPlannerContext> contextFactory)
			: base(contextFactory, DaoMapper.Instance, DaoMapper.Instance)
		{
		}

		public override Taxon Find(int key)
		{
			var builtInTaxon = s_builtIns.SingleOrDefault(n => n.Key == key);
			return builtInTaxon ?? base.Find(key);
		}

		public override Taxon Update(Taxon entity)
		{
			if (entity.IsRoot) return entity;
			return base.Update(entity);
		}

		public IEnumerable<Taxon> Handle(TaxonElementsQuery request)
		{
			if (request.TaxonKey == 0)
			{
				return s_builtIns.Where(n => n.ParentKey == 0 && n.Key != 0);
			}

			using (var context = GetContext())
			{
				return GetTable(context)
					.Where(n => n.ParentKey == request.TaxonKey)
					.AsEnumerable()
					.Select(TaxonDao.ToEntity)
					.ToList();
			}
		}
	}
}