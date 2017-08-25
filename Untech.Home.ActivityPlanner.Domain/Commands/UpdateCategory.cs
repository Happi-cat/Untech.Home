using System;
using Untech.Home.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.Home.ActivityPlanner.Domain.Commands
{

	public class UpdateCategory : ICommand<Category>
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Remarks { get; set; }
	}

}
