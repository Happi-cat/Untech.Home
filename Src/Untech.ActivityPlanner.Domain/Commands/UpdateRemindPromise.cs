using System;
using Untech.Home.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.Home.ActivityPlanner.Domain.Commands
{
	public class UpdateRemindPromise : ICommand
	{
		public int ActivityId { get; set; }
		public int Id { get; set; }
		public DateTime When { get; set; }
		public string Name { get; set; }
		public IRemindRule Rule { get; set; }
	}

}
