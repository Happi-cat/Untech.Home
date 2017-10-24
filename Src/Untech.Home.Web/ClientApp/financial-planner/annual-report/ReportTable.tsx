import * as React from 'react';
import * as classNames from 'classnames';
import { ITaxonAnnualFinancialReport, IMonthlyFinancialReport } from './Models';
import './ReportTable.less';
import { pluralizeMonth } from '../Utils';
import Money from '../components/Money';

function getMonthlyReportKey(report: IMonthlyFinancialReport) {
  return report.year + '-' + report.month;
}

interface IReportTableProps {
  entries: ITaxonAnnualFinancialReport[];
  months: IMonthlyFinancialReport[];
  onMonthClick(taxonId: number, year: number, month: number): void;
}

export class ReportTable extends React.Component<IReportTableProps, {}> {
  constructor(props: any) {
    super(props);

    this.onMonthClick = this.onMonthClick.bind(this);
  }

  public render() {
    return <table className='report-table'>
      <thead className='report-table__head'>
        <tr className='report-table__row'>
          <th></th>
          {this.renderHeaderColumns()}
        </tr>
      </thead>

      <ReportBody model={this.props.entries} onClick={this.onMonthClick} />

      <tfoot>
        <tr className='report-table__row'>
          <th className='report-heading'>Totals</th>
          {this.renderFooterColumns()}
        </tr>
      </tfoot>
    </table>;
  }

  renderHeaderColumns() {
    return this.props.months.map(monthTotalReport => <ReportMonth key={getMonthlyReportKey(monthTotalReport)}
      taxonId={0}
      model={monthTotalReport}
      onClick={this.onMonthClick}>
      <div className='report-month__month'>{pluralizeMonth(monthTotalReport.month)}</div>
      <div className='report-month__year'>{monthTotalReport.year}</div>
    </ReportMonth>);
  }

  renderFooterColumns() {
    return this.props.months.map(monthTotalReport => <ReportMonthMoney key={getMonthlyReportKey(monthTotalReport)}
      taxonId={0}
      model={monthTotalReport}
      onClick={this.onMonthClick} />);
  }

  onMonthClick(taxonId: number, year: number, month: number) {
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
  constructor() {
    super();
    this.state = { expandedTaxons: [] };

    this.toggleExpanded = this.toggleExpanded.bind(this);
  }

  public render() {
    return <tbody>
      {this.props.model.map(e => this.renderRowAndChilds(0, e))}
    </tbody>;
  }

  renderRowAndChilds(level: number, model: ITaxonAnnualFinancialReport) {
    const expanded = this.state.expandedTaxons.indexOf(model.taxonId) > -1;

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
      ? <span className='report-heading__icon--collapse' >-</span>
      : <span className='report-heading__icon--expand' >+</span>;

    let heading = model.elements && model.elements.length
      ? <th className='report-heading report-heading--clickable' onClick={() => this.toggleExpanded(model.taxonId)}>{icon}{model.name}</th>
      : <th className='report-heading'>{model.name}</th>;

    return <tr key={model.taxonId} className={cls}>
      {heading}

      {model.months.map(monthReport => <ReportMonthMoney key={getMonthlyReportKey(monthReport)}
        taxonId={model.taxonId}
        model={monthReport}
        onClick={this.props.onClick}
      />)}
    </tr>;
  }

  toggleExpanded(taxonId: number) {
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
  onClick(taxonId: number, year: number, month: number): void;
}

class ReportMonth extends React.Component<IReportMonthProps, {}> {
  constructor(props: any) {
    super(props);

    this.onClick = this.onClick.bind(this);
  }

  public render() {
    const { isPast, isNow } = this.props.model;
    const cls = classNames(
      'report-month',
      isPast && 'report-month--past',
      isNow && 'report-month--now'
    );

    return <td className={cls} children={this.props.children} onClick={this.onClick} />;
  }

  onClick() {
    this.props.onClick(this.props.taxonId, this.props.model.year, this.props.model.month);
  }
}

class ReportMonthMoney extends React.Component<IReportMonthProps, {}> {
  public render() {
    let { isPast, isNow, actual, forecasted } = this.props.model;
    actual = actual || { amount: 0, currency: { id: 'BYN' } };
    forecasted = forecasted || { amount: 0, currency: { id: 'BYN' } };

    if (isPast) {
      return <ReportMonth {...this.props}>
        <Money className='report-month__actual_money' amount={actual.amount} currencyCode={actual.currency.id} />
      </ReportMonth>;
    }

    if (isNow) {
      return <ReportMonth {...this.props}>
        <Money className='report-month__actual_money' amount={actual.amount} currencyCode={actual.currency.id} />
        &nbsp;|&nbsp;
        <Money className='report-month__forecasted_money' amount={forecasted.amount} currencyCode={actual.currency.id} />
      </ReportMonth>;
    }

    return <ReportMonth {...this.props}>
      <Money className='report-month__forecasted_money' amount={forecasted.amount} currencyCode={actual.currency.id} />
    </ReportMonth>;
  }
}