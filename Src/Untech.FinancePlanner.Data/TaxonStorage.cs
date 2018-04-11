using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

		public override Task<Taxon> FindAsync(int key, CancellationToken cancellationToken = default(CancellationToken))
		{
			var builtInTaxon = s_builtIns.SingleOrDefault(n => n.Key == key);

			if (builtInTaxon != null) return Task.FromResult(builtInTaxon);
			return base.FindAsync(key, cancellationToken);
		}

		public override Taxon Update(Taxon entity)
		{
			if (entity.IsRoot) return entity;
			return base.Update(entity);
		}

		public override Task<Taxon> UpdateAsync(Taxon entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (entity.IsRoot) return Task.FromResult(entity);
			return base.UpdateAsync(entity, cancellationToken);
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
					.ToList()
					.Select(TaxonDao.ToEntity);
			}
		}
	}
}