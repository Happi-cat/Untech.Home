export interface ICurrency {
  id: string
}

export interface IMoney {
  amount: number,
  currency: ICurrency
}

export interface ITaxonTree {
  key: number,
  parentKey: number,
  name: string,
  description: string,
  isSelectable: boolean,
  elements?: ITaxonTree[],
  elementsLoaded: boolean
}

export interface IAnnualFinancialReport {
  entries: ITaxonTree[],
  months: IMonthlyReport[]
}

export interface IMonthlyReport {
  year: number,
  month: number,
  actualTotals?: IMoney,
  forecastedTotals?: IMoney,
  entries: IMonthlyReportEntry[],
  isPast: boolean,
  isNow: boolean
}

export interface IMonthlyReportEntry {
  taxonKey: number,
  name: string,
  description: string,
  actual?: IMoney,
  actualTotals?: IMoney,
  forecasted?: IMoney,
  forecastedTotals?: IMoney,
  entries?: IMonthlyReportEntry[]
}

export interface IFinancialJournalEntry {
  key: number;
  taxonKey: number;
  remarks: string;
  actual: IMoney;
  forecasted: IMoney;
  when: Date;
}

export interface ICreateFinancialJournalEntryCommand {
  taxonKey: number;
  remarks?: string;
  actual?: IMoney;
  forecasted?: IMoney;
  year: number;
  month: number;
}

export interface IUpdateFinancialJournalEntryCommand {
  key: number;
  remarks?: string;
  actual?: IMoney;
  forecasted?: IMoney;
}

export interface IDeleteFinancialJournalEntryCommand {
  key: number;
}
