using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqToDB;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace Untech.FinancePlanner.Data
{
	public class TaxonStorage : IDataStorage<Taxon>,
		IQueryHandler<TaxonChildsQuery, IEnumerable<Taxon>>
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

		private readonly Func<IDataContext> _contextFactory;

		public TaxonStorage(Func<IDataContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public Taxon Find(int key)
		{
			var builtInTaxon = s_builtIns.SingleOrDefault(n => n.Key == key);
			if (builtInTaxon != null)
			{
				return builtInTaxon;
			}

			using (var context = _contextFactory())
			{
				var dto = context.GetTable<TaxonDto>().SingleOrDefault(n => n.Key == key);
				return TaxonDto.Convert(dto);
			}
		}

		public IEnumerable<Taxon> Find(Expression<Func<Taxon, bool>> predicate)
		{
			throw new NotSupportedException();
		}

		public Taxon Create(Taxon entity)
		{
			using (var context = _contextFactory())
			{
				var key = context.Insert(TaxonDto.Convert(entity));
				return Find(key);
			}
		}

		public bool Delete(Taxon entity)
		{
			using (var context = _contextFactory())
			{
				return context.Delete(TaxonDto.Convert(entity)) > 0;
			}
		}

		public Taxon Update(Taxon entity)
		{
			using (var context = _contextFactory())
			{
				context.Update(TaxonDto.Convert(entity));
				return entity;
			}
		}

		public IEnumerable<Taxon> Handle(TaxonChildsQuery request)
		{
			if (request.TaxonKey == 0)
			{
				return s_builtIns.Where(n => n.ParentKey == 0 && n.Key != 0);
			}

			using (var context = _contextFactory())
			{
				return context.GetTable<TaxonDto>()
					.Where(n => n.ParentKey == request.TaxonKey)
					.ToList()
					.Select(TaxonDto.Convert);
			}
		}
	}
}