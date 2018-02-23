using System.Collections.Generic;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests.Group
{
	public class GroupsQuery : IQuery<IEnumerable<Models.Group>>
	{

	}
}