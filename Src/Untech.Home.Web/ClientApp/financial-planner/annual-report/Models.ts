import { IMoney } from '../api/Models';

export interface ITaxonAnnualFinancialReport {
  taxonId: number;
  name: string;
  description: string;
  isSelectable: boolean;
  elements?: ITaxonAnnualFinancialReport[];
  months: IMonthlyFinancialReport[];
}

export interface IMonthlyFinancialReport {
  year: number;
  month: number;
  isPast: boolean;
  isNow: boolean;
  actual?: IMoney;
  forecasted?: IMoney;
}