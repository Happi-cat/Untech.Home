using System.Collections.Generic;
using System.Linq;
using Untech.Home.ActivityPlanner.Domain.Models;
using Untech.Home.ActivityPlanner.Domain.Queries;
using Untech.Home.ActivityPlanner.Domain.Views;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.Repos.Queryable;

namespace Untech.Home.ActivityPlanner.Business
{
	public class QueryService :
		IQueryHandler<DailyCalendarQuery, DailyCalendar>,
		IQueryHandler<MonthlyCalendarQuery, MonthlyCalendar>
	{
		private readonly IReadOnlyRepository<Category> _categories;
		private readonly IReadOnlyRepository<Activity> _activities;

		public QueryService(IReadOnlyRepository<Activity> activities, IReadOnlyRepository<Category> categories)
		{
			_categories = categories;
			_activities = activities;
		}

		public DailyCalendar Handle(DailyCalendarQuery request)
		{
			var response = new DailyCalendar { From = request.From, To = request.To, Categories = new List<CategoryDailyCalendar>() };

			foreach(var category in _categories.GetAll().ToList()) {
				var activities = _activities.GetAll()
					.Where(n => n.CategoryId == category.Id)
					.ToList();

				response.Categories.Add(new CategoryDailyCalendar {
					CategoryId = category.Id,
					Name = category.Name,
					Remarks = category.Remarks,
					Activities = null
				});
			}

			return response;
		}

		public MonthlyCalendar Handle(MonthlyCalendarQuery request)
		{
		var response = new MonthlyCalendar { From = request.From, To = request.To, Categories = new List<CategoryMonthlyCalendar>() };

			foreach(var category in _categories.GetAll().ToList()) {
				var activities = _activities.GetAll()
					.Where(n => n.CategoryId == category.Id)
					.ToList();

				response.Categories.Add(new CategoryDailyCalendar {
					CategoryId = category.Id,
					Name = category.Name,
					Remarks = category.Remarks,
					Activities = null
				});
			}

			return response;	
		}
	}
}
