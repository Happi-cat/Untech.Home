using System;
using Untech.Practices.CQRS;

namespace Untech.Home.ActivityPlanner.Domain.Commands
{
	public class ToggleActivityOccurrence : ICommand
	{
		public int ActivityId { get; set; }
		public DateTime When { get; set; }
	}
}
