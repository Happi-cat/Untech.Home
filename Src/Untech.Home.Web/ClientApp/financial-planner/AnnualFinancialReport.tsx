import * as React from 'react';
import { withRouter } from 'react-router';
import { RouteComponentProps } from 'react-router';
import { ReportTable } from './annual-report/ReportTable';
import { ITaxonAnnualFinancialReport, IMonthlyFinancialReport } from './annual-report/Models';
import { FinancialPlannerApiService } from './api/FinancialPlannerApiService';
import { IAnnualFinancialReport, ITaxonTree, IMonthlyReportEntry } from './api/Models';

interface AnnualFinancialReportState {
  originalReport?: IAnnualFinancialReport;
  transformedReport?: ITaxonAnnualFinancialReport;
  loading: boolean;
}

export class AnnualFinancialReport extends React.Component<RouteComponentProps<{}>, AnnualFinancialReportState> {
  constructor(props: any) {
    super(props);
    this.state = { loading: true };

    this.onMonthClick = this.onMonthClick.bind(this);
  }

  public componentWillMount() {
    new FinancialPlannerApiService()
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
      onMonthClick={this.onMonthClick} />;
  }

  onMonthClick(taxonId: number, year: number, month: number) {
    this.props.history.push('/financial-planner/journal/' + year + '/' + month + '/' + taxonId);
  }
}

class Mapper {
  public static transform(report: IAnnualFinancialReport): ITaxonAnnualFinancialReport {
    let transformedMonthlyReports = report.months.map(function (monthReport) {
      return {
        year: monthReport.year,
        month: monthReport.month,
        isPast: monthReport.isPast,
        isNow: monthReport.isNow,
        actualTotals: monthReport.actualTotals,
        forecastedTotals: monthReport.forecastedTotals,
        flattenizedEntries: Mapper.flattenizeMonthlyReportEntries(monthReport.entries)
      }
    });

    var entries = report.entries.map(iterator);
    var months = transformedMonthlyReports.map(function (m): IMonthlyFinancialReport {
      return {
        actualTotals: m.actualTotals,
        forecastedTotals: m.forecastedTotals,
        year: m.year,
        month: m.month,
        isPast: m.isPast,
        isNow: m.isNow
      }
    });

    return {
      taxonId: 0,
      elements: entries,
      months: months,
      isSelectable: false,
      name: 'root',
      description: ''
    };

    function iterator(taxon: ITaxonTree): ITaxonAnnualFinancialReport {
      let taxonMonthlyReports = transformedMonthlyReports.map(function (monthlyReport): IMonthlyFinancialReport {
        let report = monthlyReport.flattenizedEntries[taxon.id] || {};

        return {
          year: monthlyReport.year,
          month: monthlyReport.month,
          isPast: monthlyReport.isPast,
          isNow: monthlyReport.isNow,
          actual: report.actual,
          actualTotals: report.actualTotals,
          forecasted: report.forecasted,
          forecastedTotals: report.forecastedTotals,
        };
      });

      let taxonAnnualReport: ITaxonAnnualFinancialReport = {
        taxonId: taxon.id,
        name: taxon.name,
        description: taxon.description,
        isSelectable: taxon.isSelectable,
        months: taxonMonthlyReports
      };

      if (taxon.elements) {
        taxonAnnualReport.elements = taxon.elements.map(iterator);
      }

      return taxonAnnualReport;
    }
  }

  private static flattenizeMonthlyReportEntries(entries: IMonthlyReportEntry[]) {
    let dict: { [taxonId: number]: IMonthlyReportEntry } = {};

    entries.forEach(iterator);

    return dict;

    function iterator(entry: IMonthlyReportEntry) {
      dict[entry.taxonId] = entry;

      if (entry.entries) {
        entry.entries.forEach(iterator);
      }
    }
  }
}