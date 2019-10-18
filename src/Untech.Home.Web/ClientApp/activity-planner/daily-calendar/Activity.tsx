import * as React from "react";
import {IActivitiesViewActivity, IDailyCalendarDay} from "../api";
import {default as ActivityDay} from "./ActivityDay";
import {SmartQuickEditor} from "../components";
import {deleteActivity, updateActivity} from "../actions";
import {connect} from "react-redux";

export interface IActivityDay extends IDailyCalendarDay {
  key: string;
  isWeekend: boolean;
}

export interface IActivityProps {
  activity: IActivitiesViewActivity;
  allDays: IActivityDay[];

  onUpdateActivity: typeof updateActivity;
  onDeleteActivity: typeof deleteActivity;
}

class Activity extends React.PureComponent<IActivityProps> {
  public render() {
    const activityKey = this.props.activity.activityKey;
    const days = this.props.allDays.map(day => {
      const occurrence = day.activities.find(ma => ma.activityKey == activityKey);

      return {
        key: day.year + '-' + day.month,
        activityKey: activityKey,
        ...day,
        occurrence: occurrence
      };
    });

    return <tr className="daily-calendar__activity">
      <th/>
      <th>
        <SmartQuickEditor value={this.props.activity.name} onSave={this.handleSave} onDelete={this.handleDelete}/>
      </th>
      {days.map(m => <ActivityDay key={m.key} {...m} />)}
    </tr>;
  }

  handleSave = async (name: string) => {
    await this.props.onUpdateActivity(this.props.activity.activityKey, name);
  }

  handleDelete = async () => {
    await this.props.onDeleteActivity(this.props.activity.activityKey);
  }
}

const mapDispatchToProps = {
  onUpdateActivity: updateActivity,
  onDeleteActivity: deleteActivity
}

export default connect(() => ({}), mapDispatchToProps)(Activity);
