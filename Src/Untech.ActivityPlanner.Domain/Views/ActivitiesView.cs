using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class ActivitiesView : List<ActivitiesViewGroup>
	{
		public IReadOnlyCollection<ActivitiesViewGroup> Groups { get; set; }
	}
}