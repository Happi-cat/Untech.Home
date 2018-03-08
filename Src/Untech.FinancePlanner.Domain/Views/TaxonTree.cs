using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.FinancePlanner.Domain.Models;
using Untech.Practices.Collections;
using Untech.Practices.Linq;

namespace Untech.FinancePlanner.Domain.Views
{
	/// <summary>
	/// Represents taxon tree model
	/// </summary>
	[DataContract]
	public class TaxonTree : Taxon, IHierarchical<TaxonTree>
	{
		public TaxonTree(int key, int parentId, string name, string description = null)
			: base(key, parentId, name, description)
		{
		}

		/// <summary>
		/// Gets or sets loaded children
		/// </summary>
		/// <returns>Null - means not loaded; [] - means nothing</returns>
		[DataMember]
		public IReadOnlyCollection<TaxonTree> Elements { get; set; }

		[DataMember]
		public bool ElementsLoaded => Elements != null;

		public IEnumerable<TaxonTree> GetElements() => Elements.EmptyIfNull();
	}
}