using System;

namespace Untech.Home.ActivityPlanner.Domain.Models
{
	public class Reminder
	{
		public Reminder(RemindPromise promise, DateTime when)
		{
			Id = promise.Id;
			Name = promise.Name;
			When = when;
		}

		public int Id { get; }
		public string Name { get; }
		public DateTime When { get; }
		public bool Outdated => When.Date < DateTime.Today;
		public bool IsToday => When.Date == DateTime.Today;
	}
}
