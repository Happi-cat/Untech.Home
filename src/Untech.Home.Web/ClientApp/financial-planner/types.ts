import {
  IAnnualFinancialReport,
  IFinancialJournalEntry,
  IMoney,
  IMonthlyFinancialReport,
  ITaxonTree,
  TaxonKey
} from "./api";
import {Reducer, Thunk} from "repatch";

export interface MonthlyReportState {
  isSelected: boolean;
  selectedMonth: number;
  selectedYear: number;

  report?: IMonthlyFinancialReport;

  isFetching: boolean;
}

export interface JournalState {
  isSelected: boolean;
  selectedTaxon: number;
  selectedMonth: number;
  selectedYear: number;

  journal?: IFinancialJournalEntry[];
  taxon?: ITaxonTree;

  isFetching: boolean;
}

export interface State {
  annualReport?: IAnnualFinancialReport;

  isFetching: boolean;

  expandedEntries: TaxonKey[];

  monthlyReport: MonthlyReportState;
  journal: JournalState;
}

export interface IFinancialJournalEntryChange {
  remarks: string;
  actual?: IMoney;
  forecasted?: IMoney;
}

export type FinancialReportReducer = Reducer<State>;
export type FinancialReportThunk<T> = Thunk<State, {}, T>