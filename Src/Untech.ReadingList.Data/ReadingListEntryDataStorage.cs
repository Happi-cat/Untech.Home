using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyString;
using Untech.Home.Data;
using Untech.Practices.CQRS.Handlers;
using Untech.ReadingList.Domain.Models;
using Untech.ReadingList.Domain.Requests;
using Untech.ReadingList.Domain.Views;

namespace Untech.ReadingList.Data
{
	public class ReadingListEntryDataStorage : GenericDataStorage<ReadingListEntry>,
		IQueryHandler<ReadingListQuery, IEnumerable<ReadingListEntry>>,
		IQueryHandler<AuthorsQuery, AuthorsView>,
		IQueryHandler<AuthorsBooksQuery, IEnumerable<ReadingListEntry>>,
		IQueryHandler<AuthorsSuggestionQuery, IEnumerable<string>>,
		IQueryHandler<ReadingStatisticsQuery, ReadingStatistics>
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
						.AsEnumerable()
						.Select(author => new AuthorsViewItem(author)
						{
							CompletedBooksCount =
								GetTable(context).Count(n => n.Author == author && n.Status == ReadingListEntryStatus.Completed),
							ReadingBooksCount =
								GetTable(context).Count(n => n.Author == author && n.Status == ReadingListEntryStatus.Reading),
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

		public IEnumerable<string> Handle(AuthorsSuggestionQuery request)
		{
			List<string> authors;

			using (var context = GetContext())
			{
				authors = GetTable(context)
					.Select(n => n.Author)
					.Distinct()
					.ToList();
			}

			var options = new[]
			{
				FuzzyStringComparisonOptions.UseLevenshteinDistance,
				FuzzyStringComparisonOptions.UseOverlapCoefficient,
				FuzzyStringComparisonOptions.UseLongestCommonSubsequence,
				FuzzyStringComparisonOptions.UseLongestCommonSubstring
			};

			return authors
				.Where(n => n.ApproximatelyEquals(request.SearchString, FuzzyStringComparisonTolerance.Normal, options))
				.ToList();
		}

		public ReadingStatistics Handle(ReadingStatisticsQuery request)
		{
			var today = DateTime.Today;

			var yearStart = new DateTime(today.Year, 1, 1);
			var yearEnd = yearStart.AddYears(1);

			var monthIndexInQuarter = (today.Month - 1) % 3;
			var quarterStart = new DateTime(today.Year, today.Month - monthIndexInQuarter, 1);
			var quarterEnd = quarterStart.AddMonths(3);

			var monthStart = new DateTime(today.Year, today.Month, 1);
			var monthEnd = monthStart.AddMonths(1);

			List<DateTime> completionDates;

			using (var context = GetContext())
			{
				completionDates = GetTable(context)
					.Where(n => n.ReadingCompleted != null && yearStart <= n.ReadingCompleted && n.ReadingCompleted < yearEnd)
					.Select(n => n.ReadingCompleted.Value)
					.ToList();
			}

			return new ReadingStatistics
			{
				CompletedThisYear = completionDates.Count,
				CompletedThisQuarter = completionDates.Count(n => quarterStart <= n && n < quarterEnd),
				CompletedThisMonth = completionDates.Count(n => monthStart <= n && n < monthEnd)
			};
		}
	}
}