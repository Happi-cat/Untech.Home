import {RouteComponentProps} from "react-router";
import * as React from "react";
import { apiService, IDailyCalendar, IMonthlyCalendar } from './api';
import { MonthlyCalendar } from './monthly-calendar/MonthlyCalendar';
import { DailyCalendar } from './daily-calendar/DailyCalendar';

interface ActivityPlannerState {
  daily?: IDailyCalendar;
  monthly?: IMonthlyCalendar;
  loading: boolean;
}

export class ActivityPlanner extends React.Component<RouteComponentProps<{}>, ActivityPlannerState> {
  constructor(props: any) {
    super(props);
    this.state = {loading: true};
  }

  public componentWillMount() {
    Promise.all([
      apiService.getDailyCalendar(-20, 40),
      apiService.getMonthlyCalendar(-11, 1),
    ]).then(data => {
      this.setState({
        daily: data[0],
        monthly: data[1],
        loading: false
      });
    });
  }

  public render() {
    if (this.state.loading || !this.state.daily || !this.state.monthly) {
      return <div>Loading...</div>
    }

    return <div>
      <MonthlyCalendar calendar={this.state.monthly}/>
      <DailyCalendar calendar={this.state.daily}/>
    </div>;
  }
}