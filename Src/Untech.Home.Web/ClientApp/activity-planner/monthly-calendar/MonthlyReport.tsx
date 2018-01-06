import * as React from 'react';
import * as classNames from 'classnames';
import {
  IMonthlyCalendar,
  IMonthlyCalendarMonth,
  IActivitiesViewGroup,
  IActivitiesViewActivity
} from '../api';
import {pluralizeMonth} from '../../utils'

export interface IMonthlyCalendarProps {
  calendar: IMonthlyCalendar;
}

export class MonthlyCalendar extends React.Component<IMonthlyCalendarProps> {
  public render() {
    const monthNames = this.props.calendar.months
      .map(n => n.month)
      .map(pluralizeMonth);

    const groups = this.props.calendar.view.groups;
    const months = this.props.calendar.months;

    return <table className="monthly-report">
      <thead>
        <tr>
          <th></th>
          {monthNames.map(m => <th key={m}>{m}</th>)}
        </tr>
        </thead>
        {groups.map(g => <CalendarGroup
          key={g.name}
          group={g}
          months={months}
        />)}
    </table>;
  }
}

interface ICalendarGroupProps {
  group: IActivitiesViewGroup;
  months: IMonthlyCalendarMonth[];
}

function CalendarGroup(props: ICalendarGroupProps) {
  const {name, activities} = props.group;

  return <tbody>
    <tr className="monthly-report__group">
      <th>{name}</th>
      <td colSpan={props.months.length}></td>
    </tr>
    {activities.map(a => <CalendarActivity
      key={a.name}
      activity={a}
      months={props.months}/>)}
  </tbody>
}

interface ICalendarActivityProps {
  activity: IActivitiesViewActivity;
  months: IMonthlyCalendarMonth[];
}

function CalendarActivity(props: ICalendarActivityProps) {
  const activityKey = props.activity.activityKey;
  const months = props.months.map(m => {
    const activity = m.activities.find(ma => ma.activityKey == activityKey);

    return {
      key: m.year + '-' + m.month,
      count: activity ? activity.count : 0
    };
  });

  return <tr className="monthly-report__activity">
    <th>{props.activity.name}</th>
    {months.map(m => <CalendarActivityMonth key={m.key} count={m.count}/>)}
  </tr>;
}

function CalendarActivityMonth({ count }: { count: number }) {
  const level = count
    ?  Math.floor(Math.log2(count)) + 1
    : 0;

  const className = classNames([
    'monthly-report__month',
    level && ('monthly-report__month--level-' + level)
  ]);

  return <td className={className}>
    {count}
  </td>;
}

