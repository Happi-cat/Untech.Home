using System;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class UpdateRemindPromise : ICommand
	{
		public int ActivityId { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public string Rule { get; set; }
	}

}
