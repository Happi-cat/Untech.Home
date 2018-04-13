using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB;
using Untech.Home.Data;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;
using Untech.ReadingList.Domain.Models;
using Untech.ReadingList.Domain.Requests;

namespace Untech.ReadingList.Data
{
	public class ReadingListEntryDataStorage : GenericDataStorage<ReadingListEntry>,
		IQueryHandler<ReadingListQuery, IEnumerable<ReadingListEntry>>
	{
		public ReadingListEntryDataStorage(Func<ReadingListContext> contextFactory) : base(contextFactory)
		{
		}

		public IEnumerable<ReadingListEntry> Handle(ReadingListQuery request)
		{
			using (var context = GetContext())
			{
				var set = GetTable(context);

				if (request.Status == null)
				{
					return set
						.OrderBy(n => n.Author)
						.ThenBy(n => n.Title)
						.ToList();
				}

				return set
					.Where(n => n.Status == request.Status)
					.OrderBy(n => n.Author)
					.ThenBy(n => n.Title)
					.ToList();
			}
		}
	}
}