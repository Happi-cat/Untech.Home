import {RouteComponentProps} from "react-router";
import * as React from "react";
import {apiService, IDailyCalendar, IMonthlyCalendar} from './api';
import {MonthlyCalendar} from './monthly-calendar/MonthlyCalendar';
import {DailyCalendar, IDailyCalendarDispatcher} from './daily-calendar/DailyCalendar';

interface ActivityPlannerState {
  daily?: IDailyCalendar;
  monthly?: IMonthlyCalendar;
  loading: boolean;
}

export class ActivityPlanner extends React.Component<RouteComponentProps<{}>, ActivityPlannerState> {
  dispatcher: IDailyCalendarDispatcher;

  constructor(props: any) {
    super(props);
    this.state = {loading: true};

    this.dispatcher = ActivityPlanner.getDispatcher(this.reload);
  }

  public componentWillMount() {
    this.reload();
  }

  public render() {
    if (this.state.loading || !this.state.daily || !this.state.monthly) {
      return <div>Loading...</div>
    }

    return <div>
      <MonthlyCalendar calendar={this.state.monthly}/>
      <DailyCalendar calendar={this.state.daily} dispatcher={this.dispatcher}/>
    </div>;
  }

  reload = () => {
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

  static getDispatcher(refreshCallback: Function): IDailyCalendarDispatcher {
    return {
      onAddGroup(name: string) {
        apiService.createGroup({
          name: name
        }).then(() => {
          refreshCallback()
        });
      },
      onDeleteGroup(id: number) {
        apiService.deleteGroup(id)
          .then(() => {
            refreshCallback()
          });
      },
      onAddActivity(groupId: number, name: string) {
        apiService.createActivity({
          groupKey: groupId,
          name: name
        }).then(() => {
          refreshCallback()
        });
      },
      onUpdateActivity(id: number, name: string) {
      },
      onDelteActivity(id: number) {
        apiService.deleteActivity(id)
          .then(() => {
            refreshCallback()
          });
      },
      onToggleActivityOccurrence(activityId: number, year: number, month: number, day: number) {
        apiService.toggleActivityOccurrence({
          activityKey: activityId,
          when: `${year}-${month}-${day}T00:00:00Z`
        }).then(() => {
          refreshCallback()
        });
      },
      onActivityOccurrenceSelected(activityOccurrenceId: number) {

      }
    };
  }

}
