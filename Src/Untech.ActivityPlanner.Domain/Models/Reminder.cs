using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Untech.ActivityPlanner.Domain.Models
{
	[DataContract]
	public class Reminder
	{
		public Reminder(int id, DateTime thatDay, string name)
		{
			Id = id;
			When = thatDay.Date;
			Name = name;
		}

		[DataMember]
		public int Id { get; private set; }

		[DataMember]
		public DateTime When { get; private set; }

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public string Rule { get; set; }

		[DataMember]
		public DateTime? WhenAlarm { get; private set; }

		[DataMember]
		public bool Outdated => WhenAlarm.HasValue && WhenAlarm.Value.Date < DateTime.Today;

		[DataMember]
		public bool IsToday => WhenAlarm.HasValue && WhenAlarm.Value.Date == DateTime.Today;

		public void RecalculateAlarmDate(Activity activity)
		{
			var rule = GetRule(Rule);
			WhenAlarm = rule.GetAlarmDay(this, activity);
		}

		private static IRemindRule GetRule(string rule)
		{
			if (string.IsNullOrWhiteSpace(rule))
			{
				return new RemindThisDayRule();
			}
			if (int.TryParse(rule, out int count))
			{
				return new RemindAfterOccurrencesRule(count);
			}
			if (Parse(rule, out TimeSpan range))
			{
				return new RemindAfterPeriodRule(range);
			}

			throw new InvalidOperationException("Invalid rule");
		}

		private static bool Parse(string s, out TimeSpan value)
		{
			value = new TimeSpan();
			var elems = s.Split(' ');

			var modifiers = new Dictionary<char, int>
			{
				['m'] = 30,
				['w'] = 7,
				['d'] = 1
			};

			foreach (var elem in elems.Where(n => !string.IsNullOrEmpty(n)))
			{
				char modifier = elem.Last();

				if (!modifiers.TryGetValue(modifier, out int multiplier))
					return false;

				var elemWithoutModifier = elem.TrimEnd(modifier);
				if (!int.TryParse(elemWithoutModifier, out int count))
					return false;

				value = value.Add(TimeSpan.FromDays(count * multiplier));

				modifiers.Remove(modifier);
			}
			return true;
		}

		private interface IRemindRule
		{
			DateTime? GetAlarmDay(Reminder promise, Activity activity);
		}

		private class RemindAfterOccurrencesRule : IRemindRule
		{
			private readonly int _occurrencesCount;

			public RemindAfterOccurrencesRule(int occurrencesCount)
			{
				_occurrencesCount = occurrencesCount;
			}

			public DateTime? GetAlarmDay(Reminder reminder, Activity activity)
			{
				var occurrence = activity.Occurrences
					.Where(n => n.When >= reminder.When)
					.Skip(_occurrencesCount)
					.FirstOrDefault();

				return occurrence?.When;
			}
		}

		private class RemindThisDayRule : IRemindRule
		{
			public DateTime? GetAlarmDay(Reminder reminder, Activity activity)
			{
				return reminder.When;
			}
		}

		private class RemindAfterPeriodRule : IRemindRule
		{
			private readonly TimeSpan _period;

			public RemindAfterPeriodRule(TimeSpan period)
			{
				_period = period;
			}

			public DateTime? GetAlarmDay(Reminder reminder, Activity activity)
			{
				return reminder.When + _period;
			}
		}
	}
}
