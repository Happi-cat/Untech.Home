using System;
using System.Collections.Generic;
using Untech.Practices;

namespace Untech.ThingsPlanner.Domain.Models
{
	public class Project : Thing
	{
		private List<Thought> _thoughts = new List<Thought>();
		private List<Task> _tasks = new List<Task>();

		public DateTime Start { get; }

		public DateTime End { get; }

		public string Expectation { get; }

		public IReadOnlyList<Thought> Brainstorm { get; }

		public IReadOnlyList<Task> Tasks { get; }

		public void AddThought(string title)
		{
			_thoughts.Add(new Thought(title));
		}

		public void UpdateThought(Guid key, string newTitle)
		{
			_thoughts.RemoveAll(t => t.Key == key);

			_thoughts.Add(new Thought(key, newTitle));
		}

		public void DeleteThought(Guid key)
		{
			_thoughts.RemoveAll(t => t.Key == key);
		}

		public void AddTask(string title)
		{
			_tasks.Add(new Task(title));
		}

		public void UpdateTask(Guid key, string newTitle)
		{
			var oldTask = _tasks.Find(t => t.Key == key);

			_tasks.Remove(oldTask);
			_tasks.Add(new Task(key, newTitle, oldTask.Done));
		}

		public void CompleteTask(Guid key)
		{
			var oldTask = _tasks.Find(t => t.Key == key);

			_tasks.Remove(oldTask);
			_tasks.Add(new Task(key, oldTask.Title, done: true));
		}

		public void DeleteTask(Guid key)
		{
			_tasks.RemoveAll(t => t.Key == key);
		}

		public class Thought : ValueObject<Thought>
		{
			public Thought(string title)
			{
				Key = Guid.NewGuid();
				Title = title;
			}

			public Thought(Guid key, string title)
			{
				Key = key;
				Title = title;
			}

			public Guid Key { get; }

			public string Title { get; }

			protected override IEnumerable<object> GetEquatableProperties()
			{
				yield return Key;
			}
		}

		public class Task : ValueObject<Task>
		{
			public Task(string title)
			{
				Key = Guid.NewGuid();
				Title = title;
				Done = false;
			}

			public Task(Guid key, string title, bool done = false)
			{
				Key = key;
				Title = title;
			}

			public Guid Key { get; }

			public string Title { get; }

			public bool Done { get; }

			protected override IEnumerable<object> GetEquatableProperties()
			{
				yield return Key;
			}
		}
	}
}