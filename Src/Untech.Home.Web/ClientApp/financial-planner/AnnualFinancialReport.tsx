import * as React from 'react';
import { withRouter } from 'react-router';
import { RouteComponentProps } from 'react-router';
import { ReportTable } from './annual-report/ReportTable';
import { ITaxonAnnualFinancialReport, IMonthlyFinancialReport } from './annual-report/Models';
import { IAnnualFinancialReport, IMonthlyReportEntry, ITaxonTree, apiService } from './api';

interface AnnualFinancialReportState {
  originalReport?: IAnnualFinancialReport;
  transformedReport?: ITaxonAnnualFinancialReport;
  loading: boolean;
}

export class AnnualFinancialReport extends React.Component<RouteComponentProps<{}>, AnnualFinancialReportState> {
  constructor(props: any) {
    super(props);
    this.state = { loading: true };
  }

  public componentWillMount() {
    apiService
      .getReport()
      .then(data => {
        this.setState({
          originalReport: data,
          transformedReport: Mapper.transform(data),
          loading: false
        });
      });
  }

  public render() {
    if (this.state.loading || !this.state.transformedReport) {
      return <div>Loading...</div>
    }

    return <div>
      {this.renderReport(this.state.transformedReport)}
    </div>;
  }

  renderReport(report: ITaxonAnnualFinancialReport) {
    return <ReportTable
      entries={report.elements || []}
      months={report.months || []}
      onMonthClick={this.handleMonthClick} />;
  }

  handleMonthClick = (taxonId: number, year: number, month: number) => {
    this.props.history.push('/financial-planner/journal/' + year + '/' + month + '/' + taxonId);
  }
}

class Mapper {
  public static transform(report: IAnnualFinancialReport): ITaxonAnnualFinancialReport {
    let transformedMonthlyReports = report.months.map(function (monthReport) {
      return {
        ...monthReport,
        flattenizedEntries: Mapper.flattenizeMonthlyReportEntries(monthReport.entries)
      }
    });

    var entries = report.entries.map(iterator);
    var months = transformedMonthlyReports.map(function (m): IMonthlyFinancialReport {
      const { year, month, isPast, isNow, actualTotals, forecastedTotals } = m;

      return { actualTotals, forecastedTotals, year, month, isPast, isNow };
    });

    return {
      taxonKey: 0,
      elements: entries,
      months: months,
      isSelectable: false,
      name: 'root',
      description: ''
    };

    function iterator(taxon: ITaxonTree): ITaxonAnnualFinancialReport {
      let taxonMonthlyReports = transformedMonthlyReports.map(function (monthlyReport): IMonthlyFinancialReport {
        const { year, month, isPast, isNow } = monthlyReport;

        const report = monthlyReport.flattenizedEntries[taxon.key] || {};
        const { actual, actualTotals, forecasted, forecastedTotals } = report;

        return { year, month, isPast, isNow, actual, actualTotals, forecasted, forecastedTotals, };
      });

      return {
        taxonKey: taxon.key,
        name: taxon.name,
        description: taxon.description,
        isSelectable: taxon.isSelectable,
        months: taxonMonthlyReports,
        elements: taxon.elements ? taxon.elements.map(iterator) : undefined
      };
    }
  }

  private static flattenizeMonthlyReportEntries(entries: IMonthlyReportEntry[]) {
    let dict: { [taxonId: number]: IMonthlyReportEntry } = {};

    entries.forEach(iterator);

    return dict;

    function iterator(entry: IMonthlyReportEntry) {
      dict[entry.taxonKey] = entry;

      if (entry.entries) {
        entry.entries.forEach(iterator);
      }
    }
  }
}