using System.Runtime.Serialization;

namespace Untech.FinancePlanner.Domain.Models
{
	[DataContract]
	public class Taxon
	{
		public Taxon(int id, int parentId, string name, string description = null)
		{
			Id = id;
			ParentId = parentId;
			Name = name;
			Description = description;
		}

		[DataMember]
		public int Id { get; private set; }

		[DataMember]
		public int ParentId { get; private set; }

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public string Description { get; private set; }

		[DataMember]
		public bool IsSystemRoot => Id == 0;

		[DataMember]
		public bool IsRoot => ParentId == 0;

		/// <summary>
		/// Determines whether this taxon is selectable or not.
		/// </summary>
		[DataMember]
		public bool IsSelectable => !IsRoot;
	}
}