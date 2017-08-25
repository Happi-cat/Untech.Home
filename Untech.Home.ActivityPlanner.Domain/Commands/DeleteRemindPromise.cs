using Untech.Practices.CQRS;

namespace Untech.Home.ActivityPlanner.Domain.Commands
{
	public class DeleteRemindPromise : ICommand
	{
		public int ActivityId { get; set; }
		public int Id { get; set; }
	}
}
