import * as React from "react";
import {IActivitiesViewActivity, IMonthlyCalendarMonth} from "../api";
import ActivityMonth from "./ActivityMonth";

export interface IActivityProps {
  activity: IActivitiesViewActivity;
  months: IMonthlyCalendarMonth[];
}

export default function Activity(props: IActivityProps) {
  const activityKey = props.activity.activityKey;
  const months = props.months.map(m => {
    const activity = m.activities.find(ma => ma.activityKey == activityKey);

    return {
      key: m.year + '-' + m.month,
      count: activity ? activity.count : 0,
      isThisMonth: m.isThisMonth
    };
  });

  return <tr className="monthly-calendar__activity">
    <th>{props.activity.name}</th>

    {months.map(m => <ActivityMonth
      key={m.key}
      count={m.count}
      isThisMonth={m.isThisMonth}
    />)}
  </tr>;
}