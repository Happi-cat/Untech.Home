import { IMoney } from '../api/Models';

export { IMoney };

export interface ITaxonAnnualFinancialReport {
  taxonKey: number;
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
  actualTotals?: IMoney;
  forecasted?: IMoney;
  forecastedTotals?: IMoney;
}