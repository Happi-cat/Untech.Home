using System;
using System.Collections.Generic;
using System.Linq;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Domain.Requests;
using Untech.ActivityPlanner.Domain.Requests.Activity;
using Untech.ActivityPlanner.Domain.Requests.ActivityOccurrence;
using Untech.ActivityPlanner.Domain.Requests.Group;
using Untech.ActivityPlanner.Domain.Views;
using Untech.Home;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;

namespace Untech.ActivityPlanner.Domain.Services
{
	public class QueryService : IQueryHandler<ActivitiesViewQuery, ActivitiesView>,
		IQueryHandler<DailyCalendarQuery, DailyCalendar>,
		IQueryHandler<MonthlyCalendarQuery, MonthlyCalendar>
	{
		private readonly IQueryDispatcher _dispatcher;

		public QueryService(IQueryDispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		public ActivitiesView Handle(ActivitiesViewQuery request) => new ActivitiesView
		{
			Groups = _dispatcher
				.Fetch(new GroupsQuery())
				.Select(group => new ActivitiesViewGroup(group)
				{
					Activities = _dispatcher
						.Fetch(new ActivitiesQuery(group.Key))
						.Select(n => new ActivitiesViewActivity(n))
						.ToList()
				})
				.ToList()
		};

		public DailyCalendar Handle(DailyCalendarQuery request)
		{
			var view = Handle(new ActivitiesViewQuery());

			var occurrencesPerDay = _dispatcher
				.Fetch(request.Occurrences)
				.ToKeyValues(n => n.When.Date);

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

		public MonthlyCalendar Handle(MonthlyCalendarQuery request)
		{
			var view = Handle(new ActivitiesViewQuery());

			var occurrencesPerMonth = _dispatcher
				.Fetch(request.Occurrences)
				.ToKeyValues(n => n.When.AsMonthDate());

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