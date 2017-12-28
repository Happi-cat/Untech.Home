using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Untech.ActivityPlanner.Domain.Models;
using Untech.ActivityPlanner.Domain.Requests;
using Untech.ActivityPlanner.Domain.Views;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.Home.Web.Controllers
{
	[Route("api/activity-planner")]
	public class ActivityPlannerController : Controller
	{
		private readonly IDispatcher _dispatcher;

		public ActivityPlannerController(IDispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		[HttpGet("calendar/daily/{fromDay}-{toDay}")]
		public DailyCalendar GetDailyCalendar(int fromDay, int toDay) => _dispatcher
			.Fetch(new DailyCalendarQuery(fromDay, toDay));

		[HttpGet("calendar/monthly/{fromMonth}-{toMonth}")]
		public MonthlyCalendar GetMonthlyCalendar(int fromMonth, int toMonth) => _dispatcher
			.Fetch(new MonthlyCalendarQuery(fromMonth, toMonth));

		[HttpPost("group")]
		public Group CreateGroup([FromBody]CreateGroup request) => _dispatcher.Process(request);

		[HttpDelete("group/{key}")]
		public bool DeleteGroup(int key) => _dispatcher.Process(new DeleteGroup(key));

		[HttpPost("activity")]
		public Activity CreateActivity([FromBody]CreateActivity request) => _dispatcher.Process(request);

		[HttpDelete("activity/{key}")]
		public bool DeleteActivity(int key) => _dispatcher.Process(new DeleteActivity(key));

		[HttpPost("activity/{key}/toggle-occurrence")]
		public void ToggleActivityOccurrence(int key, [FromBody]DateTime occurrence)
		{
			_dispatcher.Process(new ToggleActivityOccurrence(key, occurrence));
		}

		[HttpPost("activity/{key}/toggle-occurrences")]
		public void ToogleActivityOccurrences(int key, [FromBody]DateTime[] occurrences)
		{
			var requests = occurrences
				.Select(n => n.Date)
				.GroupBy(n => n)
				.Where(n => n.Count() % 2 == 1)
				.Select(n => new ToggleActivityOccurrence(key, n.Key));

			foreach (var request in requests)
			{
				_dispatcher.Process(request);
			}
		}

		[HttpPut("occurrence/{key}")]
		public void UpdateActivityOccurrence(int key, [FromBody] UpdateActivityOccurrence request)
		{
			if (request.Key != key) throw new ArgumentException("request.Key is invalid and doesn't match to key");

			_dispatcher.Process(request);
		}
	}
}
