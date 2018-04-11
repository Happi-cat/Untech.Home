using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.Views;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace Untech.FinancePlanner.Domain.Services
{
	public class TaxonQueryService : IQueryAsyncHandler<TaxonTreeQuery, TaxonTree>
	{
		private readonly IQueryDispatcher _dispatcher;
		private readonly IAsyncDataStorage<Taxon> _dataStorage;

		public TaxonQueryService(IAsyncDataStorage<Taxon> dataStorage, IQueryDispatcher dispatcher)
		{
			_dispatcher = dispatcher;
			_dataStorage = dataStorage;
		}

		public async Task<TaxonTree> HandleAsync(TaxonTreeQuery request, CancellationToken cancellationToken)
		{
			var taxon = await _dataStorage.FindAsync(request.TaxonKey, cancellationToken);

			return new TaxonTree(taxon.Key, taxon.ParentKey, taxon.Name, taxon.Description)
			{
				Elements = await GetDescendantsAsync(taxon.Key, new Depth(request.Deep), cancellationToken)
			};
		}

		private async Task<List<TaxonTree>> GetDescendantsAsync(int parentTaxonKey, Depth depth, CancellationToken cancellationToken)
		{
			if (depth.IsOver) return null;

			depth = depth.Decrement();

			var elements = await GetElementsAsync(parentTaxonKey, cancellationToken);

			if (depth.IsOver) return elements;

			foreach (var taxon in elements)
			{
				taxon.Elements = await GetDescendantsAsync(taxon.Key, depth, cancellationToken);
			}
			return elements;
		}

		private async Task<List<TaxonTree>> GetElementsAsync(int parentTaxonKey, CancellationToken cancellationToken)
		{
			var taxonElements = await _dispatcher
				.FetchAsync(new TaxonElementsQuery(parentTaxonKey), cancellationToken);

			return taxonElements
				.Select(n => new TaxonTree(n.Key, n.ParentKey, n.Name, n.Description))
				.ToList();
		}

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