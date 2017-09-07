using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.FinancePlanner.Domain.Models;

namespace Untech.FinancePlanner.Domain.ViewModels
{
	/// <summary>
	/// Represents taxon tree model
	/// </summary>
	[DataContract]
	public class TaxonTree : Taxon, IHierarchical<TaxonTree>
	{
		public TaxonTree(int id, int parentId, string name, string description = null)
			: base(id, parentId, name, description)
		{
		}

		/// <summary>
		/// Determines whether this taxon is selectable or not.
		/// </summary>
		[DataMember]
		public bool IsSelectable => !IsRoot;

		/// <summary>
		/// Gets or sets loaded children
		/// </summary>
		/// <returns></returns>
		[DataMember]
		public IReadOnlyCollection<TaxonTree> Elements { get; set; }
	}
}