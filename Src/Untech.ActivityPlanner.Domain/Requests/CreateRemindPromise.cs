using System;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class CreateRemindPromise : ICommand<RemindPromise>
	{
		public int ActivityId { get; set; }
		public DateTime When { get; set; }
		public string Name { get; set; }
		public string Rule { get; set; }
	}

}
