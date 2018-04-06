import * as React from 'react';
import {IMonthlyFinancialReport, IMonthlyFinancialReportDayEntry} from '../api/Models';
import {MoneyView, MonthView} from '../components';
import {IMonthlyFinancialReportDay} from "../api";
import './ReportTable.less';

interface IMonthlyReportTableProps {
  report: IMonthlyFinancialReport;

  onTaxonClick(taxonId: number): void;
}

export class MonthlyReportTable extends React.Component<IMonthlyReportTableProps, {}> {
  public render() {
    const { year, month } = this.props.report;
    return <table className="monthly-table-report">
      <thead>
        <tr>
          <th>
            Category
          </th>
          <th>
            Remarks
          </th>
          <th>
            Actual
          </th>
          <th>
            Forecasted
          </th>
        </tr>
      </thead>

      {this.props.report.days.map(reportDay => this.renderDay(year, month, reportDay))}
    </table>;
  }

  renderDay(year: number, month: number, day: IMonthlyFinancialReportDay) {

    return <tbody key={day.day}>
    <tr className="monthly-table-report-day">
      <td colSpan={2}>
        <MonthView year={year} month={month} />
        {day.day}
      </td>
      <td>
        <MoneyView amount={day.actualTotals.amount} currencyCode={day.actualTotals.currency.id}/>
      </td>
      <td>
        <MoneyView amount={day.forecastedTotals.amount} currencyCode={day.forecastedTotals.currency.id}/>
      </td>
    </tr>

    {day.entries.map((entry, index) => this.renderEntry(entry, index))}
    </tbody>
  }

  renderEntry(entry: IMonthlyFinancialReportDayEntry, index: number) {
    let {actual, forecasted} = entry;
    actual = actual || {amount: 0, currency: {id: 'BYN'}};
    forecasted = forecasted || {amount: 0, currency: {id: 'BYN'}};


    return <tr key={index} onClick={() => this.handleMonthClick(entry.taxonKey)}>
      <td>
        {entry.name}
      </td>
      <td>
        {entry.remarks}</td>
      <td>
        <MoneyView amount={actual.amount} currencyCode={actual.currency.id}/>
      </td>
      <td>
        <MoneyView amount={forecasted.amount} currencyCode={forecasted.currency.id}/>
      </td>
    </tr>;
  }

  handleMonthClick = (taxonId: number) => {
    this.props.onTaxonClick(taxonId);
  }
}