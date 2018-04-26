using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Untech.Practices;

namespace Untech.ThingsPlanner.Domain.Models
{
	public abstract partial class Thing
	{
		[DataContract]
		public sealed class Project : Thing
		{
			private readonly List<ProjectItem> _items = new List<ProjectItem>();

			public Project(int key, string title) : base(ThingType.Project, key, title)
			{
			}

			[DataMember]
			public DateTime? Start { get; set; }

			[DataMember]
			public DateTime? End { get; set; }

			[DataMember]
			public string Expectation { get; }

			[DataMember]
			public IReadOnlyCollection<ProjectItem> Items => _items;

			[DataMember]
			public IEnumerable<ProjectItem> Thoughts => _items.Where(n => n.Type == ProjectItemType.Thought);

			[DataMember]
			public IEnumerable<ProjectItem> Tasks => _items.Where(n => n.Type == ProjectItemType.Task);

			public void AddItem(ProjectItemType type, string title)
			{
				_items.Add(new ProjectItem(type, title));
			}

			public void UpdateItem(Guid key, string newTitle, bool done = false)
			{
				var oldItem = _items.Single(n => n.Key == key);

				_items.Remove(oldItem);

				_items.Add(new ProjectItem(key, oldItem.Type, newTitle, done));
			}

			public void ChangeItemType(Guid key, ProjectItemType newType)
			{
				var oldItem = _items.Single(n => n.Key == key);

				_items.Remove(oldItem);

				_items.Add(new ProjectItem(key, newType, oldItem.Title));
			}

			public void DeleteItem(Guid key)
			{
				_items.RemoveAll(t => t.Key == key);
			}

			public enum ProjectItemType
			{
				Thought = 1,
				Task = 2
			}

			[DataContract]
			public class ProjectItem : ValueObject<ProjectItem>
			{
				public ProjectItem(ProjectItemType type, string title)
				{
					Key = Guid.NewGuid();
					Type = type;
					Title = title;
				}

				public ProjectItem(Guid key, ProjectItemType type, string title, bool done = false)
				{
					Key = key;
					Type = type;
					Title = title;
					Done = done;
				}

				[DataMember]
				public Guid Key { get; private set; }

				[DataMember]
				public ProjectItemType Type { get; private set; }

				[DataMember]
				public string Title { get; private set; }

				[DataMember]
				public bool Done { get; private set; }

				protected override IEnumerable<object> GetEquatableProperties()
				{
					yield return Key;
				}
			}
		}
	}
}