using Untech.Home.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.Home.ActivityPlanner.Domain.Commands
{
	public class CreateActivity : ICommand<Activity>
	{
		public string Name { get; set; }
		public string Remarks { get; set; }
		public int CategoryId { get; set; }
	}
}
