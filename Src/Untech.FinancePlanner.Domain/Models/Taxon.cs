using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace Untech.FinancePlanner.Domain.Models
{
	[DataContract]
	public class Taxon : IAggregateRoot
	{
		protected Taxon() { }

		public Taxon(int Key, int parentKey, string name, string description = null)
		{
			this.Key = Key;
			ParentKey = parentKey;
			Name = name;
			Description = description;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public int ParentKey { get; private set; }

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public string Description { get; private set; }

		[DataMember]
		public bool IsSystemRoot => Key == 0;

		[DataMember]
		public bool IsRoot => ParentKey == 0;

		/// <summary>
		/// Determines whether this taxon is selectable or not.
		/// </summary>
		[DataMember]
		public bool IsSelectable => !IsRoot;
	}
}