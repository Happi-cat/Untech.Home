using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
		public Task<DailyCalendar> GetDailyCalendar(int fromDay, int toDay)
		{
			return _dispatcher
				.FetchAsync(new DailyCalendarQuery(fromDay, toDay), CancellationToken.None);
		}

		[HttpGet("calendar/monthly/{fromMonth}-{toMonth}")]
		public Task<MonthlyCalendar> GetMonthlyCalendar(int fromMonth, int toMonth)
		{
			return _dispatcher
				.FetchAsync(new MonthlyCalendarQuery(fromMonth, toMonth), CancellationToken.None);
		}

		[HttpPost("group")]
		public Task<Group> CreateGroup([FromBody]CreateGroup request)
		{
			return _dispatcher.ProcessAsync(request, CancellationToken.None);
		}

		[HttpPut("group/{key}")]
		public Task<Group> UpdateGroup(int key, [FromBody]UpdateGroup request)
		{
			if (key != request.Key) throw new ArgumentException("request.Key is invalid and doesn't match to key");

			return _dispatcher.ProcessAsync(request, CancellationToken.None);
		}

		[HttpDelete("group/{key}")]
		public Task<bool> DeleteGroup(int key)
		{
			return _dispatcher.ProcessAsync(new DeleteGroup(key), CancellationToken.None);
		}

		[HttpPost("activity")]
		public Task<Activity> CreateActivity([FromBody]CreateActivity request)
		{
			return _dispatcher.ProcessAsync(request, CancellationToken.None);
		}

		[HttpPut("activity/{key}")]
		public Task<Activity> UpdateActivity(int key, [FromBody]UpdateActivity request)
		{
			if (key != request.Key) throw new ArgumentException("request.Key is invalid and doesn't match to key");

			return _dispatcher.ProcessAsync(request, CancellationToken.None);
		}

		[HttpDelete("activity/{key}")]
		public Task<bool> DeleteActivity(int key)
		{
			return _dispatcher.ProcessAsync(new DeleteActivity(key), CancellationToken.None);
		}

		[HttpPost("activity/{key}/toggle-occurrence")]
		public Task ToggleActivityOccurrence(int key, [FromBody]DateTime occurrence)
		{
			return _dispatcher.ProcessAsync(new ToggleActivityOccurrence(key, occurrence), CancellationToken.None);
		}

		[HttpPost("activity/{key}/toggle-occurrences")]
		public Task ToogleActivityOccurrences(int key, [FromBody]DateTime[] occurrences)
		{
			var requests = occurrences
				.Select(n => n.Date)
				.GroupBy(n => n)
				.Where(n => n.Count() % 2 == 1)
				.Select(n => new ToggleActivityOccurrence(key, n.Key));

			return Task.WhenAll(requests.Select(n => _dispatcher.ProcessAsync(n, CancellationToken.None)));
		}

		[HttpPut("occurrence/{key}")]
		public Task UpdateActivityOccurrence(int key, [FromBody] UpdateActivityOccurrence request)
		{
			if (request.Key != key) throw new ArgumentException("request.Key is invalid and doesn't match to key");

			return _dispatcher.ProcessAsync(request, CancellationToken.None);
		}
	}
}
