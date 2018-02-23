using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.ActivityPlanner.Domain.Requests.Activity
{
	[DataContract]
	public class CreateActivity : ICommand<Models.Activity>
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
