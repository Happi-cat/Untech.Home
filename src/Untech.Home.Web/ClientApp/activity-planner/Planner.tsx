import {RouteComponentProps} from "react-router";
import * as React from "react";
import {apiService, IActivityOccurrence, IDailyCalendar, IMonthlyCalendar} from './api';
import {MonthlyCalendar} from './monthly-calendar/MonthlyCalendar';
import {DailyCalendar} from './daily-calendar/DailyCalendar';
import {HorScrollable} from './components/HorScrollable';
import {OccurrenceEditor} from "./occurrence-editor/OccurrenceEditor";

interface ActivityPlannerState {
  daily?: IDailyCalendar;
  monthly?: IMonthlyCalendar;
  selectedOccurrence?: IActivityOccurrence;
  loading: boolean;
}

export class ActivityPlanner extends React.Component<RouteComponentProps<{}>, ActivityPlannerState> {
  constructor(props: any) {
    super(props);
    this.state = {loading: true};
  }

  public componentWillMount() {
    this.reload();
  }

  public render() {
    if (this.state.loading || !this.state.daily || !this.state.monthly) {
      return <div>Loading...</div>
    }

    return <div>
      <HorScrollable>
        <MonthlyCalendar calendar={this.state.monthly}/>
      </HorScrollable>

      <HorScrollable>
        <DailyCalendar calendar={this.state.daily}/>
      </HorScrollable>

      {this.state.selectedOccurrence && <OccurrenceEditor
        occurrence={this.state.selectedOccurrence}
        onSubmit={this.handleOccurrenceSubmit}
      />}
    </div>;
  }
}
