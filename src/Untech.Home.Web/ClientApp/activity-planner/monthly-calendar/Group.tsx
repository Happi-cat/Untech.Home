import * as React from "react";
import {IActivitiesViewGroup, IMonthlyCalendarMonth} from "../api";
import Activity from "./Activity";

export interface IGroupProps {
  group: IActivitiesViewGroup;
  months: IMonthlyCalendarMonth[];
}

export default class CalendarGroup extends React.PureComponent<IGroupProps> {
  public render() {
    const {name, activities} = this.props.group;

    return <tbody>
    <tr className="monthly-calendar__group">
      <th>{name}</th>
      <td colSpan={this.props.months.length}></td>
    </tr>

    {activities.map(a => <Activity
      key={a.name}
      activity={a}
      months={this.props.months}
    />)}
    </tbody>
  }
}
