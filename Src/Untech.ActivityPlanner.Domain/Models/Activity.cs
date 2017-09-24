using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Untech.ActivityPlanner.Domain.Models;

namespace Untech.ActivityPlanner.Domain.Models
{
	[DataContract]
	public class Activity
	{
		private readonly List<ActivityOccurrence> _occurrences;
		private readonly List<Note> _notes;

		private readonly List<Reminder> _reminders;

		public Activity(int id, int groupId, string name)
		{
			Id = id;
			GroupId = groupId;
			Name = name;

			_occurrences = new List<ActivityOccurrence>();
			_notes = new List<Note>();
			_reminders = new List<Reminder>();
		}

		[DataMember]
		public int Id { get; private set; }

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public string Remarks { get; set; }

		[DataMember]
		public int GroupId { get; private set; }

		[DataMember]
		public IReadOnlyCollection<ActivityOccurrence> Occurrences => _occurrences;

		[DataMember]
		public IReadOnlyCollection<Note> Notes { get; set; }

		[DataMember]
		public IReadOnlyCollection<Reminder> Reminders { get; set; }

		public void ToggleOccurrence(DateTime when)
		{
			if (_occurrences.Contains(when))
			{
				_occurrences.Remove(when);
			}
			else
			{
				_occurrences.Add(when);
			}

			_reminders.ForEach(n => n.RecalculateAlarmDate(this));
		}

		public void AddOrUpdateNote(DateTime when, string remarks)
		{
			_notes.RemoveAll(n => n.When == when);
			_notes.Add(new Note(when) { Remarks = remarks });
		}

		public void DeleteNote(DateTime when)
		{
			_notes.RemoveAll(n => n.When == when);
		}

		public void AddReminder(DateTime when, string rule, string name)
		{
			var reminder = new Reminder(0, when, name)
			{
				Rule = rule
			};

			reminder.RecalculateAlarmDate(this);
			_reminders.Add(reminder);
		}

		public void UpdateReminder(int id, DateTime when, string rule, string name)
		{
			var existingPromise = _reminders.Single(n => n.Id == id);
			// existing.Name = name;
			existingPromise.Rule = rule;

			existingPromise.RecalculateAlarmDate(this);
		}

		public void DeleteReminderer(int id)
		{
			_reminders.RemoveAll(n => n.Id == id);
		}
	}
}
