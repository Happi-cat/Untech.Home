using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.Requests.Taxon;
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
				Elements = GetDescendants(taxon.Key, new Depth(request.Deep))
			};
		}

		private List<TaxonTree> GetDescendants(int parentTaxonKey, Depth depth)
		{
			if (depth.IsOver) return null;

			depth = depth.Decrement();

			var elements = GetElements(parentTaxonKey);

			if (depth.IsOver) return elements;

			foreach (var taxon in elements)
			{
				taxon.Elements = GetDescendants(taxon.Key, depth);
			}
			return elements;
		}

		private List<TaxonTree> GetElements(int parentTaxonKey) => _dispatcher
			.Fetch(new TaxonElementsQuery(parentTaxonKey))
			.Select(n => new TaxonTree(n.Key, n.ParentKey, n.Name, n.Description))
			.ToList();

		/// <summary>
		/// -1 - infinitely; 0 - over; +n - n levels.
		/// </summary>
		private struct Depth
		{
			private int _deep;

			public Depth(int deep) => _deep = deep;

			public Depth Decrement() => new Depth(_deep > 0 ? --_deep : _deep);

			public bool IsOver => _deep == 0;
		}

	}
}