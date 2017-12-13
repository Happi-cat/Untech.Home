using System.Runtime.Serialization;
using Untech.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests
{
	[DataContract]
	public class CreateActivity : ICommand<Activity>
	{
		public CreateActivity(int groupKey, string name)
		{
			GroupKey = groupKey;
			Name = name;
		}

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public int GroupKey { get; private set; }
	}
}
