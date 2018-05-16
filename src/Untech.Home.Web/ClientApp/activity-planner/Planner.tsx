import * as React from "react";
import {IDailyCalendar, IMonthlyCalendar} from './api';
import MonthlyCalendar from './monthly-calendar';
import DailyCalendar from './daily-calendar';
import {HorScrollable} from './components';
import OccurrenceEditor from "./occurrence-editor";
import {Loader} from "semantic-ui-react";
import {connect} from "react-redux";
import {State} from "./types";

interface IActivityPlannerProps {
  dailyCalendar?: IDailyCalendar;
  monthlyCalendar?: IMonthlyCalendar;
  isOccurrenceSelected: boolean;
  isLoading: boolean;
}

class ActivityPlanner extends React.PureComponent<IActivityPlannerProps> {
  constructor(props: any) {
    super(props);
    this.state = {isLoading: true, isOccurrenceSelected: false};
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

const mapStateToProps = (state: State) => ({
  dailyCalendar: state.dailyCalendar,
  monthlyCalendar: state.monthlyCalendar,
  isOccurrenceSelected: !!state.selectedActivityOccurrence ,
  isLoading: state.isFetching
});

export default connect(mapStateToProps, {})(ActivityPlanner);
