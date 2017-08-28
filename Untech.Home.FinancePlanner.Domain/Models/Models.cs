namespace Untech.Home.FinancePlanner.Domain.Models
{
	public class Currency {
		public string Name {get;set;}
		public string Code {get;set;}
	}

	public class Money 
	{
		public double Amount { get;set; }
		public Currency Currency {get;set;}
	}

	public enum FinanceLogEntryType {
		Expense,
		Saving,
		Income
	}

	public class FinanceLogEntry
	{
		public int Id {get;set;}
		public int TaxonId { get;set; }
		public FinanceLogEntryType Type { get;set; }
		public string Remarks { get;set; }
		public Money Spent { get;set; }
		public DateTim When { get;set; }
		public Money Forecasted { get;set; }
	}

	public class Taxon {
		public int Id { get; set; }
		public int? ParentId {get;set;}
		public string Name {get;set;}
	}
}