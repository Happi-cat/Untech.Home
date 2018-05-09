export type TaxonKey = number;
export type FinancialJournalEntryKey = number;

export interface ICurrency {
  id: string
}

export interface IMoney {
  amount: number,
  currency: ICurrency
}

export interface ITaxonTree {
  key: TaxonKey,
  parentKey: TaxonKey,
  name: string,
  description: string,
  isSelectable: boolean,
  elements?: ITaxonTree[],
  elementsLoaded: boolean
}

export interface IAnnualFinancialReport {
  entries: ITaxonTree[],
  months: IAnnualFinancialReportMonth[]
}

export interface IAnnualFinancialReportMonth {
  year: number,
  month: number,
  actualTotals?: IMoney,
  forecastedTotals?: IMoney,
  entries: IAnnualFinancialReportMonthEntry[],
  isPast: boolean,
  isNow: boolean
}

export interface IAnnualFinancialReportMonthEntry {
  taxonKey: TaxonKey,
  name: string,
  description: string,
  actual?: IMoney,
  actualTotals?: IMoney,
  forecasted?: IMoney,
  forecastedTotals?: IMoney,
  entries?: IAnnualFinancialReportMonthEntry[]
}

export interface IMonthlyFinancialReport {
  year: number;
  month: number;
  days: IMonthlyFinancialReportDay[];
}

export interface IMonthlyFinancialReportDay {
  day: number;
  actualTotals: IMoney;
  forecastedTotals: IMoney;
  entries: IMonthlyFinancialReportDayEntry[];
}

export interface IMonthlyFinancialReportDayEntry {
  name: string;
  taxonKey: TaxonKey;
  remarks?: string;
  actual?: IMoney;
  forecasted?: IMoney;
}

export interface IFinancialJournalEntry {
  key: FinancialJournalEntryKey;
  taxonKey: TaxonKey;
  remarks: string;
  actual: IMoney;
  forecasted: IMoney;
  when: Date;
}

export interface ICreateFinancialJournalEntryCommand {
  taxonKey: TaxonKey;
  remarks?: string;
  actual?: IMoney;
  forecasted?: IMoney;
  year: number;
  month: number;
}

export interface IUpdateFinancialJournalEntryCommand {
  key: FinancialJournalEntryKey;
  remarks?: string;
  actual?: IMoney;
  forecasted?: IMoney;
}

export interface IDeleteFinancialJournalEntryCommand {
  key: FinancialJournalEntryKey;
}
