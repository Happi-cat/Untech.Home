using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.ActivityPlanner.Domain.Models;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class ActivitiesViewGroup
	{
		public ActivitiesViewGroup(Group group)
		{
			GroupKey = group.Key;
			Name = group.Name;
		}

		[DataMember]
		public int GroupKey { get; private set; }

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public IReadOnlyCollection<ActivitiesViewActivity> Activities { get; set; }
	}
}