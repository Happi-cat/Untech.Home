using System;
using Untech.Home.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.Home.ActivityPlanner.Domain.Commands
{
	public class CreateCategory : ICommand<Category>
	{
		public string Name { get; set; }
		public string Remarks { get; set; }
	}

}
