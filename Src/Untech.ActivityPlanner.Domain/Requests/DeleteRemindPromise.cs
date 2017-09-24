using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class DeleteRemindPromise : ICommand
	{
		public int ActivityId { get; set; }
		public int Id { get; set; }
	}
}
