using System;
using System.Runtime.Serialization;
using Untech.Practices.Core;
using Untech.Practices.DataStorage;

namespace Untech.ReadingList.Domain.Models
{
	[DataContract]
	public class ReadingListEntry : IAggregateRoot
	{
		private ReadingListEntry()
		{
		}

		public ReadingListEntry(int key, string author, string title)
		{
			Key = key;
			Author = author.NullIfEmpty() ?? throw new ArgumentNullException(nameof(author));
			Title = title.NullIfEmpty() ?? throw new ArgumentNullException(nameof(title));
		}

		[DataMember]
		public int Key { get; }

		[DataMember]
		public string Author { get; private set; }

		[DataMember]
		public string Title { get; private set; }

		[DataMember]
		public ReadingListEntryStatus Status { get; private set; }

		[DataMember]
		public DateTime? ReadingStarted { get; private set; }

		[DataMember]
		public DateTime? ReadingCompleted { get; private set; }

		[DataMember]
		public TimeSpan DaysReading
		{
			get
			{
				switch (Status)
				{
					case ReadingListEntryStatus.Reading:
						return DateTime.UtcNow - ReadingStarted.Value;
					case ReadingListEntryStatus.Completed:
						return ReadingCompleted.Value - ReadingStarted.Value;
				}

				return TimeSpan.Zero;
			}
		}

		public void StartReading()
		{
			Status = ReadingListEntryStatus.Reading;
			ReadingStarted = DateTime.UtcNow;
		}

		public void CompleteReading()
		{
			if (Status != ReadingListEntryStatus.Reading)
			{
				throw new InvalidOperationException("User must start reading book first");
			}

			Status = ReadingListEntryStatus.Completed;
			ReadingCompleted = DateTime.UtcNow;
		}
	}
}