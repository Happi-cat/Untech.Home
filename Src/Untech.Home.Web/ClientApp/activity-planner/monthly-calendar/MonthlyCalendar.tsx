import * as React from 'react';
import * as classNames from 'classnames';
import './MonthlyCalendar.less';
import {
  IMonthlyCalendar,
  IMonthlyCalendarMonth,
  IActivitiesViewGroup,
  IActivitiesViewActivity
} from '../api';
import { pluralizeMonth } from '../../utils'

export interface IMonthlyCalendarProps {
  calendar: IMonthlyCalendar;
}

export class MonthlyCalendar extends React.PureComponent<IMonthlyCalendarProps> {
  public render() {
    const months = this.props.calendar.months;

    const headers = months.map(m => ({
      key: m.year + '-' + m.month,
      name: pluralizeMonth(m.month),
      isThisMonth: m.isThisMonth
    }));

    const groups = this.props.calendar.view.groups;

    return <table className="monthly-calendar">
      <thead>
        <tr>
          <th />
          {headers.map(m => <Month key={m.key} isThisMonth={m.isThisMonth}>
            {m.name}
          </Month>)}
        </tr>
      </thead>

      {groups.map(g => <Group
        key={g.groupKey}
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

function Group(props: ICalendarGroupProps) {
  const { name, activities } = props.group;

  return <tbody>
    <tr className="group">
      <th>{name}</th>
      <td colSpan={props.months.length}></td>
    </tr>

    {activities.map(a => <Activity
      key={a.activityKey}
      activity={a}
      months={props.months}
    />)}
  </tbody>
}

interface ICalendarMonthProps {
  isThisMonth: boolean;
  className?: string;
}

class Month extends React.PureComponent<ICalendarMonthProps> {
  public render() {
    const className = classNames([
      'month',
      this.props.isThisMonth && '-this',
      this.props.className
    ]);

    return <td className={className}>
      {this.props.children}
    </td>;
  }
}

interface ICalendarActivityProps {
  activity: IActivitiesViewActivity;
  months: IMonthlyCalendarMonth[];
}

function Activity(props: ICalendarActivityProps) {
  const activityKey = props.activity.activityKey;
  const months = props.months.map(m => {
    const activity = m.activities.find(ma => ma.activityKey == activityKey);

    return {
      key: m.year + '-' + m.month,
      count: activity ? activity.count : 0,
      isThisMonth: m.isThisMonth
    };
  });

  return <tr className="activity">
    <th>{props.activity.name}</th>

    {months.map(m => <ActivityMonth
      key={m.key}
      count={m.count}
      isThisMonth={m.isThisMonth}
    />)}
  </tr>;
}

function ActivityMonth({ count, isThisMonth }: { count: number, isThisMonth: boolean }) {
  const level = count ? Math.floor(Math.log2(count)) + 1 : 0;

  const className = classNames([
    level && ('-level-' + level)
  ]);

  return <Month isThisMonth={isThisMonth} className={className}>
    {count ? count : null}
  </Month>;
}
