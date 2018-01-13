using System.Collections.Generic;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	public class GroupsQuery : IQuery<IEnumerable<Group>>
	{

	}
}