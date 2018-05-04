namespace Untech.Books.Librus.Models
{
	/// <summary>
	/// Represents additional field that not fits into <see cref="BookInfo"/> model
	/// but should be indexed.
	/// </summary>
	public class MetaField
	{
		/// <summary>
		/// Field name. Will be available for search.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Field value.
		/// </summary>
		public string Value { get; set; }
	}
}