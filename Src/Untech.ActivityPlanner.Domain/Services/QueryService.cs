using System;
using System.Collections.Generic;
using System.Linq;
using Untech.ActivityPlanner.Domain.Requests;
using Untech.ActivityPlanner.Domain.Views;
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

			var occurrences = _dispatcher
				.Fetch(request.Occurrences)
				.GroupBy(n => n.When.Date)
				.ToDictionary(n => n.Key, n => n.ToArray());

			var allDays = GetDates(request.Occurrences.From, request.Occurrences.To, n => n.AddDays(1))
				.Select(day => new DailyCalendarDay(day)
				{
					Activities = occurrences.ContainsKey(day) ? occurrences[day].ToArray() : new Models.ActivityOccurrence[0]
				});

			return new DailyCalendar(view)
			{
				Months = allDays
					.GroupBy(n => new DateTime(n.Year, n.Month, 1))
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

			var occurrences = _dispatcher
				.Fetch(request.Occurrences)
				.GroupBy(n => new DateTime(n.When.Year, n.When.Month, 1))
				.ToDictionary(n => n.Key, n => n.ToArray());

			var dates = GetDates(request.Occurrences.From, request.Occurrences.To, n => n.AddMonths(1));

			return new MonthlyCalendar(view)
			{
				Months = dates
					.Select(month => new MonthlyCalendarMonth(month)
					{
						Activities = occurrences.ContainsKey(month)
							? occurrences[month]
								.GroupBy(n => n.ActivityKey)
								.Select(n => new MonthlyCalendarMonthActivity(n.Key, n.Count(m => !m.Missed)))
								.ToArray()
							: new MonthlyCalendarMonthActivity[0]
					})
					.ToList()
			};
		}

		private IEnumerable<DateTime> GetDates(DateTime from, DateTime to, Func<DateTime, DateTime> stepper)
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