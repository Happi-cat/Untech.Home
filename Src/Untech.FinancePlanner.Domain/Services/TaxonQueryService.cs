using System.Collections.Generic;
using System.Linq;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.ViewModels;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace Untech.FinancePlanner.Domain.Services
{
	public class TaxonQueryService : IQueryHandler<TaxonTreeQuery, TaxonTree>
	{
		private readonly IQueryDispatcher _dispatcher;
		private readonly IDataStorage<Taxon> _dataStorage;

		public TaxonQueryService(IDataStorage<Taxon> dataStorage, IQueryDispatcher dispatcher)
		{
			_dispatcher = dispatcher;
			_dataStorage = dataStorage;
		}

		public TaxonTree Handle(TaxonTreeQuery request)
		{
			var taxon = _dataStorage.Find(request.TaxonKey);

			return new TaxonTree(taxon.Key, taxon.ParentKey, taxon.Name, taxon.Description)
			{
				Elements = GetDescendants(taxon.Key, request.Deep)
			};
		}

		private List<TaxonTree> GetDescendants(int parentTaxonKey, int deep)
		{
			if (deep == 0) return null;

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
			return _dispatcher.Fetch(new TaxonElementsQuery(parentTaxonKey))
				.Select(n => new TaxonTree(n.Key, n.ParentKey, n.Name, n.Description))
				.ToList();
		}

		private int DecrementDeep(int deep) => (deep > 0) ? --deep : deep;
	}
}