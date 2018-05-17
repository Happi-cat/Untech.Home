import * as React from "react";
import {IDailyCalendar, IMonthlyCalendar} from './api';
import MonthlyCalendar from './monthly-calendar';
import DailyCalendar from './daily-calendar';
import {HorScrollable, Loader} from './components';
import OccurrenceEditor from "./occurrence-editor";
import {connect} from "react-redux";
import {State} from "./types";
import {fetchCalendar} from "./actions";

interface IActivityPlannerProps {
  dailyCalendar?: IDailyCalendar;
  monthlyCalendar?: IMonthlyCalendar;
  isOccurrenceSelected: boolean;
  isLoading: boolean;

  onMount: typeof fetchCalendar;
}

class ActivityPlanner extends React.PureComponent<IActivityPlannerProps> {
  constructor(props: any) {
    super(props);
    this.state = {isLoading: true, isOccurrenceSelected: false};
  }

  componentDidMount() {
    this.props.onMount();
  }

  public render() {
    if (this.props.isLoading || !this.props.dailyCalendar || !this.props.monthlyCalendar) {
      return <div>
        <Loader>Loading...</Loader>
      </div>
    }

    return <div>
      <HorScrollable>
        <MonthlyCalendar calendar={this.props.monthlyCalendar}/>
      </HorScrollable>

      <HorScrollable>
        <DailyCalendar calendar={this.props.dailyCalendar}/>
      </HorScrollable>

      {this.props.isOccurrenceSelected && <OccurrenceEditor/>}
    </div>;
  }
}

const mapStateToProps = (state: State) : Partial<IActivityPlannerProps> => ({
  dailyCalendar: state.dailyCalendar,
  monthlyCalendar: state.monthlyCalendar,
  isOccurrenceSelected: !!state.selectedActivityOccurrence,
  isLoading: state.isFetching
});

const mapDispatchToProps = {
  onMount: fetchCalendar
}

export default connect(mapStateToProps, mapDispatchToProps)(ActivityPlanner);
