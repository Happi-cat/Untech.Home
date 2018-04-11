using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Domain.Requests;
using Untech.ActivityPlanner.Domain.Views;
using Untech.Home;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;

namespace Untech.ActivityPlanner.Domain.Services
{
	public class QueryService : IQueryAsyncHandler<ActivitiesViewQuery, ActivitiesView>,
		IQueryAsyncHandler<DailyCalendarQuery, DailyCalendar>,
		IQueryAsyncHandler<MonthlyCalendarQuery, MonthlyCalendar>
	{
		private readonly IQueryDispatcher _dispatcher;

		public QueryService(IQueryDispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		public async Task<ActivitiesView> HandleAsync(ActivitiesViewQuery request, CancellationToken cancellationToken)
		{
			var groups = await _dispatcher.FetchAsync(new GroupsQuery(), cancellationToken);
			var activitiesPerGroup = await Task.WhenAll(groups
				.Select(group => _dispatcher
					.FetchAsync(new ActivitiesQuery(group.Key), cancellationToken)));

			return new ActivitiesView
			{
				Groups = groups.Select((group, i) => new ActivitiesViewGroup(group)
					{
						Activities = activitiesPerGroup[i]
							.Select(n => new ActivitiesViewActivity(n))
							.ToList()
					})
					.ToList()
			};
		}

		public async Task<DailyCalendar> HandleAsync(DailyCalendarQuery request, CancellationToken cancellationToken)
		{
			var view = await HandleAsync(new ActivitiesViewQuery(), cancellationToken);

			var occurrences = await _dispatcher.FetchAsync(request.Occurrences, cancellationToken);
			var occurrencesPerDay = occurrences.ToKeyValues(n => n.When.Date);

			return new DailyCalendar(view)
			{
				Months = GetDays(request.Occurrences)
					.Select(day => new DailyCalendarDay(day)
					{
						Activities = occurrencesPerDay.GetValues(day).ToArray()
					})
					.GroupBy(n => n.AsMonthDate())
					.Select(group => new DailyCalendarMonth(group.Key)
					{
						Days = group.OrderBy(n => n.Day).ToArray()
					})
					.ToArray()
			};
		}

		public async Task<MonthlyCalendar> HandleAsync(MonthlyCalendarQuery request, CancellationToken cancellationToken)
		{
			var view = await HandleAsync(new ActivitiesViewQuery(), cancellationToken);

			var occurrences = await _dispatcher.FetchAsync(request.Occurrences, cancellationToken);
			var occurrencesPerMonth = occurrences.ToKeyValues(n => n.When.AsMonthDate());

			return new MonthlyCalendar(view)
			{
				Months = GetMonths(request.Occurrences)
					.Select(month => new MonthlyCalendarMonth(month)
					{
						Activities = occurrencesPerMonth
							.GetValues(month)
							.GroupBy(n => n.ActivityKey)
							.Select(n => new MonthlyCalendarMonthActivity(n.Key, n.Count(m => !m.Missed)))
							.ToArray()
					})
					.ToList()
			};
		}

		private static IEnumerable<DateTime> GetDays(OccurrencesQuery query) =>
			GetDates(query.From, query.To, n => n.AddDays(1));

		private static IEnumerable<DateTime> GetMonths(OccurrencesQuery query) =>
			GetDates(query.From, query.To, n => n.AddMonths(1));

		private static IEnumerable<DateTime> GetDates(DateTime from, DateTime to, Func<DateTime, DateTime> stepper)
		{
			if (from > to) yield break;
			if (from == to)
			{
				yield return from;
				yield break;
			}

			var current = from;
			while (current < to)
			{
				yield return current;
				current = stepper(current);
			}
		}
	}
}