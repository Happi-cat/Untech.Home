using System;
using Untech.Home.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.Home.ActivityPlanner.Domain.Commands
{
	public class UpdateActivityOccurrenceRemarks : ICommand<Occurrence>
	{
		public int ActivityId { get; set; }
		public DateTime When { get; set; }
		public string Remarks { get; set; }
	}

}
