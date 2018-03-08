import * as React from 'react';
import { withRouter } from 'react-router';
import { MonthlyReportTable } from './monthly-report/ReportTable';
import { RouteComponentProps } from 'react-router';
import { IMonthlyFinancialReport, apiService } from './api';

interface IMonthlyFinancialReportProps {
  year: string;
  month: string;
}

interface IMonthlyFinancialReportState {
  loading: boolean;
  report?: IMonthlyFinancialReport;
}

export class MonthlyFinancialReport extends React.Component<RouteComponentProps<IMonthlyFinancialReportProps>, IMonthlyFinancialReportState> {
  constructor(props: any) {
    super(props);
    this.state = { loading: true };
  }

  public componentWillMount() {
    let { year, month } = this.props.match.params;

    apiService
      .getMonthlyReport(year, month)
      .then(data => {
        this.setState({
          report: data,
          loading: false
        });
      });
  }

  public render() {
    if (this.state.loading || !this.state.report) {
      return <div>Loading...</div>
    }

    return <div>
      <MonthlyReportTable report={this.state.report} onTaxonClick={this.handleTaxonClick} />
    </div>;
  }

  handleTaxonClick = (taxonId: number) => {
    let { year, month } = this.props.match.params;

    this.props.history.push('/financial-planner/journal/' + year + '/' + month + '/' + taxonId);
  }
}