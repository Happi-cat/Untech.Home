using System;
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
						.Select(_ => new ActivitiesViewActivity(_))
						.ToList()
				})
				.ToList()
		};

		public DailyCalendar Handle(DailyCalendarQuery request)
		{
			var view = Handle(new ActivitiesViewQuery());

			var occurrences = _dispatcher
				.Fetch(request.OccurrencesQuery)
				.GroupBy(_ => _.When.Date);

			return new DailyCalendar(view)
			{
				Days = occurrences
					.Select(dayOccurrences => new DailyCalendarDay(dayOccurrences.Key)
					{
						Activities = dayOccurrences
							.Select(_ => new DailyCalendarDayActivity(_.ActivityKey, _))
							.ToList()
					})
					.ToList()
			};
		}

		public MonthlyCalendar Handle(MonthlyCalendarQuery request)
		{
			var view = Handle(new ActivitiesViewQuery());

			var occurrences = _dispatcher
				.Fetch(request.Occurrences)
				.GroupBy(_ => new DateTime(_.When.Year, _.When.Month, 1));

			return new MonthlyCalendar(view)
			{
				Months = occurrences
					.Select(monthOccurrences => new MonthlyCalendarMonth(monthOccurrences.Key)
					{
						Activities = monthOccurrences
							.GroupBy(_ => _.ActivityKey)
							.Select(_ => new MonthlyCalendarMonthActivity(_.Key, _.Count()))
							.ToList()
					})
					.ToList()
			};
		}
	}
}