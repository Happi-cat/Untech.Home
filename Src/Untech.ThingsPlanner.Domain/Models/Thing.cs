using Untech.Practices.DataStorage;

namespace Untech.ThingsPlanner.Domain.Models
{
	public class Thing : IAggregateRoot
	{
		public int Key { get; }

		public string Title { get; }

		public ThingStatus Status { get; }
	}
}