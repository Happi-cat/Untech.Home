using System.Runtime.Serialization;
using Untech.ActivityPlanner.Domain.Models;

namespace Untech.ActivityPlanner.Domain.Views
{
	[DataContract]
	public class ActivitiesViewActivity
	{
		public ActivitiesViewActivity(Activity activity)
		{
			ActivityKey = activity.Key;
			Name = activity.Name;
		}

		[DataMember]
		public int ActivityKey { get; private set; }

		[DataMember]
		public string Name { get; private set; }
	}
}