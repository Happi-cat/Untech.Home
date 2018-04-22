import {RouteComponentProps} from "react-router";
import * as React from "react";
import {apiService, IActivityOccurrence, IDailyCalendar, IMonthlyCalendar} from './api';
import {MonthlyCalendar} from './monthly-calendar/MonthlyCalendar';
import {DailyCalendar, IDailyCalendarDispatcher} from './daily-calendar/DailyCalendar';
import {HorScrollable} from './components/HorScrollable';
import {OccurrenceEditor} from "./occurrence-editor/OccurrenceEditor";

interface ActivityPlannerState {
  daily?: IDailyCalendar;
  monthly?: IMonthlyCalendar;
  selectedOccurrence?: IActivityOccurrence;
  loading: boolean;
}

export class ActivityPlanner extends React.Component<RouteComponentProps<{}>, ActivityPlannerState> {
  dispatcher: IDailyCalendarDispatcher;

  constructor(props: any) {
    super(props);
    this.state = {loading: true};

    this.dispatcher = this.getDispatcher();
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
        <DailyCalendar calendar={this.state.daily} dispatcher={this.dispatcher}/>
      </HorScrollable>

      {this.state.selectedOccurrence && <OccurrenceEditor
        occurrence={this.state.selectedOccurrence}
        onSubmit={this.handleOccurrenceSubmit}
      />}
    </div>;
  }

  reload = () => {
    Promise.all([
      apiService.getDailyCalendar(-3 * 7, 6 * 7),
      apiService.getMonthlyCalendar(-21, 3),
    ]).then(data => {
      this.setState({
        daily: data[0],
        monthly: data[1],
        loading: false
      });
    });
  }

  handleOccurrenceSubmit = (occurrence : IActivityOccurrence) => {
    apiService.updateActivityOccurrence({
      key: occurrence.key,
      note: occurrence.note,
      highlighted: occurrence.highlighted,
      missed: occurrence.missed
    }).then(() => {
      this.reload();
    });
  }

  getDispatcher(): IDailyCalendarDispatcher {
    var self = this;

    return {
      onAddGroup(name: string) {
        apiService.createGroup({
          name: name
        }).then(() => {
          self.reload();
        });
      },
      onUpdateGroup(id: number,  name: string) {
        apiService.updateGroup({
          key: id,
          name: name
        }).then(() => {
          self.reload();
        });
      },
      onDeleteGroup(id: number) {
        apiService.deleteGroup(id)
          .then(() => {
            self.reload();
          });
      },
      onAddActivity(groupId: number, name: string) {
        apiService.createActivity({
          groupKey: groupId,
          name: name
        }).then(() => {
          self.reload();
        });
      },
      onUpdateActivity(id: number, name: string) {
        apiService.updateActivity({
          key: id,
          name: name
        }).then(() => {
          self.reload();
        });
      },
      onDelteActivity(id: number) {
        apiService.deleteActivity(id)
          .then(() => {
            self.reload();
          });
      },
      onToggleActivityOccurrence(activityId: number, year: number, month: number, day: number) {
        apiService.toggleActivityOccurrence({
          activityKey: activityId,
          when: `${year}-${month}-${day}T00:00:00Z`
        }).then(() => {
          self.reload();
        });
      },
      onActivityOccurrenceSelected(occurrence: IActivityOccurrence) {
        self.setState({selectedOccurrence: occurrence});
      }
    }
  }
}
