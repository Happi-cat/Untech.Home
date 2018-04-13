using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB;
using Untech.Home.Data;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;
using Untech.ReadingList.Domain.Models;
using Untech.ReadingList.Domain.Requests;
using Untech.ReadingList.Domain.Views;

namespace Untech.ReadingList.Data
{
	public class ReadingListEntryDataStorage : GenericDataStorage<ReadingListEntry>,
		IQueryHandler<ReadingListQuery, IEnumerable<ReadingListEntry>>,
		IQueryHandler<AuthorsQuery, AuthorsView>,
		IQueryHandler<AuthorsBooksQuery, IEnumerable<ReadingListEntry>>
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

		public AuthorsView Handle(AuthorsQuery request)
		{
			using (var context = GetContext())
			{
				return new AuthorsView
				{
					Auhtors = GetTable(context)
						.Select(n => n.Author)
						.Distinct()
						.ToList()
						.Select(author => new AuthorsViewItem(author)
						{
							CompletedBooksCount = GetTable(context).Count(n => n.Author == author && n.Status == ReadingListEntryStatus.Completed),
							ReadingBooksCount = GetTable(context).Count(n => n.Author == author && n.Status == ReadingListEntryStatus.Reading),
							TotalBooksCount = GetTable(context).Count(n => n.Author == author),
						})
						.ToList()
				};
			}
		}

		public IEnumerable<ReadingListEntry> Handle(AuthorsBooksQuery request)
		{
			using (var context = GetContext())
			{
				return GetTable(context)
					.Where(n => n.Author == request.Author)
					.OrderBy(n => n.Author)
					.ThenBy(n => n.Title)
					.ToList();
			}
		}
	}
}