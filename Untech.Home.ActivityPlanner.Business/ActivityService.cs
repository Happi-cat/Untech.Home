using System;
using System.Linq;
using Untech.Home.ActivityPlanner.Domain.Commands;
using Untech.Home.ActivityPlanner.Domain.Models;
using Untech.Practices;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.Repos.Queryable;

namespace Untech.Home.ActivityPlanner.Business
{
	public class ActivityService :
		ICommandHandler<CreateActivity, Activity>,
		ICommandHandler<UpdateActivity, Unit>,
		ICommandHandler<ToggleActivityOccurrence, Unit>,
		ICommandHandler<UpdateActivityOccurrenceRemarks, Occurrence>
	{
		private readonly IRepository<Activity> _activities;

		public ActivityService(IRepository<Activity> activities)
		{
			_activities = activities;
		}

		public Activity Handle(CreateActivity request)
		{
			return _activities.Create(new Activity
			{
				Name = request.Name,
				Remarks = request.Remarks,
				CategoryId = request.CategoryId
			});
		}

		public Unit Handle(UpdateActivity request)
		{
			var activity = _activities.GetAll().Single(n => n.Id == request.Id);

			activity.Name = request.Name;
			activity.Remarks = request.Remarks;

			_activities.Update(activity);

			return Unit.Value;
		}

		public Unit Handle(ToggleActivityOccurrence request)
		{
			throw new NotImplementedException();
		}

		public Occurrence Handle(UpdateActivityOccurrenceRemarks request)
		{
			throw new NotImplementedException();
		}
	}
}
