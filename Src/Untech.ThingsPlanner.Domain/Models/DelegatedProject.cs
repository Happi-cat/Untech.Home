using System;

namespace Untech.ThingsPlanner.Domain.Models
{
	public class DelegatedProject : Thing
	{
		public DateTime FollowUp { get; }

		public string Person { get; }
	}
}