using System.Runtime.Serialization;

namespace Untech.ThingsPlanner.Domain.Models
{
	public abstract partial class Thing
	{
		[DataContract]
		public sealed class Idea : Thing
		{
			public Idea(int key, string title)
				: base(ThingType.Idea, key, title)
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

			public SomedayProject ConvertToSomedayProject()
			{
				return new SomedayProject(Key, Title);
			}
		}
	}
}