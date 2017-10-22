export interface ICurrency {
  id: string
}

export interface IMoney {
  amount: number,
  currency: ICurrency
}

export interface ITaxonTree {
  id: number,
  parentId: number,
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
  actualBalance?: IMoney,
  forecastedBalance?: IMoney,
  entries: IMonthlyReportEntry[],
  isPast: boolean,
  isNow: boolean
}

export interface IMonthlyReportEntry {
  taxonId: number,
  name: string,
  description: string,
  actual?: IMoney,
  forecasted?: IMoney,
  entries?: IMonthlyReportEntry[]
}

export interface IFinancialJournalEntry {
  id: number;
  taxonId: number;
  remarks: string;
  actual: IMoney;
  forecasted: IMoney;
  when: Date;
}

export interface ICreateFinancialJournalEntryCommand {
  taxonId: number;
  remarks?: string;
  actual?: IMoney;
  forecasted?: IMoney;
  year: number;
  month: number;
}

export interface IUpdateFinancialJournalEntryCommand {
  id: number;
  remarks?: string;
  actual?: IMoney;
  forecasted?: IMoney;
}

export interface IDeleteFinancialJournalEntryCommand {
  id: number;
}
