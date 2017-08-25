using Untech.Practices.CQRS;

namespace Untech.Home.ActivityPlanner.Domain.Commands
{
	public class UpdateActivity : ICommand
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Remarks { get; set; }
	}
}
