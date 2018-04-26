import * as React from 'react';
import * as classNames from 'classnames';
import { IMoney, ITaxonAnnualFinancialReport, IMonthlyFinancialReport } from './Models';
import './ReportTable.less';
import { MoneyView, MonthView } from '../components';
import { Icon } from 'semantic-ui-react';

function getMonthlyReportKey(report: IMonthlyFinancialReport) {
  return report.year + '-' + report.month;
}

interface IReportTableProps {
  entries: ITaxonAnnualFinancialReport[];
  months: IMonthlyFinancialReport[];
  onMonthClick(taxonId: number, year: number, month: number): void;
}

export class ReportTable extends React.Component<IReportTableProps, {}> {
  public render() {
    return <table className='report-table'>
      <thead className='report-table__head'>
        <tr className='report-table__row'>
          <th></th>
          {this.renderHeaderColumns()}
        </tr>
      </thead>

      <ReportBody model={this.props.entries} onClick={this.handleMonthClick} />

      <tfoot>
        <tr className='report-table__row report-table__row--footer'>
          <th className='report-heading'>Totals</th>
          {this.renderFooterColumns()}
        </tr>
      </tfoot>
    </table>;
  }

  renderHeaderColumns() {
    return this.props.months.map(monthTotalReport => <ReportCell key={getMonthlyReportKey(monthTotalReport)}
      taxonId={0}
      model={monthTotalReport}
      onClick={this.handleMonthClick}>
      <MonthView year={monthTotalReport.year} month={monthTotalReport.month} />
    </ReportCell>);
  }

  renderFooterColumns() {
    return this.props.months.map(monthTotalReport => <ReportMoneyCell key={getMonthlyReportKey(monthTotalReport)}
      showTotals
      taxonId={0}
      model={monthTotalReport}
      onClick={this.handleMonthClick} />);
  }

  handleMonthClick = (taxonId: number, year: number, month: number) => {
    this.props.onMonthClick(taxonId, year, month);
  }
}

interface IReportBodyProps {
  model: ITaxonAnnualFinancialReport[],
  onClick(taxonId: number, year: number, month: number): void;
}

interface IReportBodyState {
  expandedTaxons: number[]
}

class ReportBody extends React.Component<IReportBodyProps, IReportBodyState> {
  constructor(props: any) {
    super(props);

    this.state = { expandedTaxons: [] };
  }

  public render() {
    return <tbody>
      {this.props.model.map(e => this.renderRowAndChilds(0, e))}
    </tbody>;
  }

  renderRowAndChilds(level: number, model: ITaxonAnnualFinancialReport) {
    const expanded = this.state.expandedTaxons.indexOf(model.taxonKey) > -1;

    let expandedRows: React.ReactNode[] = expanded && model.elements
      ? model.elements.map(m => this.renderRowAndChilds(level + 1, m))
      : [];

    return [
      this.renderRow(level, model, expanded),
      ...expandedRows
    ];
  }

  renderRow(level: number, model: ITaxonAnnualFinancialReport, expanded?: boolean) {
    const cls = classNames(
      'report-table__row',
      'report-table__row--l' + level,
    );

    let icon = expanded
      ? <Icon name='triangle down' />
      : <Icon name='triangle left' />

    let heading = model.elements && model.elements.length
      ? <th className='report-heading -clickable' onClick={() => this.toggleExpanded(model.taxonKey)}>{icon}{model.name}</th>
      : <th className='report-heading'>{model.name}</th>;

    return <tr key={model.taxonKey} className={cls}>
      {heading}

      {model.months.map(monthReport => <ReportMoneyCell key={getMonthlyReportKey(monthReport)}
        taxonId={model.taxonKey}
        showTotals={!expanded}
        model={monthReport}
        onClick={this.props.onClick}
      />)}
    </tr>;
  }

  toggleExpanded = (taxonId: number) => {
    this.setState(function (prevState, props) {
      const expanded = prevState.expandedTaxons.indexOf(taxonId) > -1;
      const expandedTaxons = expanded
        ? prevState.expandedTaxons.filter(e => e != taxonId)
        : prevState.expandedTaxons.concat(taxonId);

      return { expandedTaxons: expandedTaxons };
    });
  }
}

interface IReportMonthProps {
  taxonId: number;
  model: IMonthlyFinancialReport;
  showTotals?: boolean;
  onClick(taxonId: number, year: number, month: number): void;
}

class ReportCell extends React.Component<IReportMonthProps, {}> {
  public render() {
    const { isPast, isNow } = this.props.model;
    const cls = classNames('report-month', {
      '-past': isPast,
      '-now': isNow
    });

    return <td className={cls} children={this.props.children} onClick={this.handleClick} />;
  }

  handleClick = () => {
    this.props.onClick(this.props.taxonId, this.props.model.year, this.props.model.month);
  }
}

class ReportMoneyCell extends React.Component<IReportMonthProps, {}> {
  public render() {
    const { isPast, isNow } = this.props.model;
    const { actual, forecasted } = this;
    const showActual = isPast || isNow;
    const showForecasted = !isPast || isNow;

    return <ReportCell {...this.props}>
      {showActual && <div className='report-month__actual-money'>
        <MoneyView amount={actual.amount} currencyCode={actual.currency.id} />
      </div>}
      {showForecasted && <div className='report-month__forecasted-money'>
        <MoneyView amount={forecasted.amount} currencyCode={actual.currency.id} />
      </div>}
    </ReportCell>;
  }

  get actual() {
    let { actual, actualTotals } = this.props.model;

    return this.selectMoney(actual, actualTotals);
  }

  get forecasted() {
    let { forecasted, forecastedTotals } = this.props.model;

    return this.selectMoney(forecasted, forecastedTotals);
  }

  selectMoney(money?: IMoney, totals?: IMoney) {
    money = this.props.showTotals ? totals : money;
    return money || { amount: 0, currency: { id: 'BYN' } };
  }
}