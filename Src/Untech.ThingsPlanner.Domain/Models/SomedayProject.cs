using System.Runtime.Serialization;

namespace Untech.ThingsPlanner.Domain.Models
{
	public abstract partial class Thing
	{
		[DataContract]
		public sealed class SomedayProject: Thing
		{
			public SomedayProject(int key, string title) : base(ThingType.SomedayProject, key, title)
			{
			}

			public Project ConvertIntoProject()
			{
				return new Project(Key, Title);
			}

			public DelegatedProject ConvertIntoDelegatedProject()
			{
				return new DelegatedProject(Key, Title);
			}
		}
	}
}