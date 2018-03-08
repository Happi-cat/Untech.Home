import * as React from 'react';
import { IMonthlyFinancialReport, IMonthlyFinancialReportDayEntry } from '../api/Models';
import { MoneyView } from '../components';

interface IMonthlyReportTableProps {
  report: IMonthlyFinancialReport;
  onTaxonClick(taxonId : number): void;
}

export class MonthlyReportTable extends React.Component<IMonthlyReportTableProps, {}> {
  public render() {
    return <div>
      {this.props.report.days.map(reportDay => <div key={reportDay.day}>
        <h3><span>{reportDay.day}</span>
          <span> - </span>
          <MoneyView amount={reportDay.actualTotals.amount} currencyCode={reportDay.actualTotals.currency.id} />
          <span> / </span>
          <MoneyView amount={reportDay.forecastedTotals.amount} currencyCode={reportDay.forecastedTotals.currency.id} />
        </h3>

        <ul>
          {reportDay.entries.map((entry, index) => this.renderEntry(entry, index))}
        </ul>
      </div>)}
    </div>;
  }

  renderEntry(entry: IMonthlyFinancialReportDayEntry, index: number) {
    let { actual, forecasted } = entry;
    actual = actual || { amount: 0, currency: { id: 'BYN' } };
    forecasted = forecasted || { amount: 0, currency: { id: 'BYN' } };


    return <li key={index} onClick={() => this.handleMonthClick(entry.taxonKey)}>
      <span>{entry.name}</span><span> - </span>
      <span>{entry.remarks}</span>
      <span> - </span>
      <MoneyView amount={actual.amount} currencyCode={actual.currency.id} />
      <span> / </span>
      <MoneyView amount={forecasted.amount} currencyCode={forecasted.currency.id} />
    </li>;
  }

  handleMonthClick = (taxonId: number) => {
    this.props.onTaxonClick(taxonId);
  }
}