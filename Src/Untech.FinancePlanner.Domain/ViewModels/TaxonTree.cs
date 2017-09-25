using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.Collections;
using Untech.FinancePlanner.Domain.Models;
using Untech.Practices.Linq;
using System.Collections;

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

		public IEnumerator<TaxonTree> GetEnumerator()
		{
			return Elements.EmptyIfNull().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}