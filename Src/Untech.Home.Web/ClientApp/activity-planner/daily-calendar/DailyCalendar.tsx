import * as React from 'react';
import * as classNames from 'classnames';
import {
  IDailyCalendar,
  IDailyCalendarDay,
  IActivitiesViewGroup,
  IActivitiesViewActivity, IActivityOccurrence
} from '../api';
import {pluralizeDayOfWeek, pluralizeMonth} from '../../utils'

export interface IDailyCalendarProps {
  calendar: IDailyCalendar;
}

export class DailyCalendar extends React.Component<IDailyCalendarProps> {
  public render() {
    const months = this.props.calendar.months.map(m => ({
      key: m.year + '-' + m.month,
      name: pluralizeMonth(m.month),
      daysCount: m.days.length,
      days: m.days
    }));
    const days = months.map(m => m.days)
      .reduce((o, n) => o.concat(n), [])
      .map(d => ({
        key: d.year + '-' + d.month + '-' + d.day,
        isWeekend: d.dayOfWeek >= 6,
        ...d
      }));
    const groups = this.props.calendar.view.groups;

    return <table className="monthly-report">
      <thead>
        <tr>
          <th></th>
          {months.map(m => <th key={m.key} colSpan={m.daysCount}>{m.name}</th>)}
        </tr>
        <tr>
          <th></th>
          {days.map(d => <CalendarDay key={d.key} isWeekend={d.isWeekend}>
            {d.day}
          </CalendarDay>)}
        </tr>
        <tr>
          <th></th>
          {days.map(d => <CalendarDay key={d.key} isWeekend={d.isWeekend}>
            {pluralizeDayOfWeek(d.dayOfWeek)}
          </CalendarDay>)}
        </tr>
      </thead>
      {groups.map(g => <CalendarGroup
        key={g.name}
        group={g}
        allDays={days}
      />)}
    </table>;
  }
}

interface IExtendendDailyCalendarDay extends IDailyCalendarDay {
  key: string;
  isWeekend: boolean;
}

interface ICalendarDayProps {
  isWeekend: boolean;
  className?: string;
}

class CalendarDay extends React.PureComponent<ICalendarDayProps> {
  public render() {
    const className = classNames([
      'daily-calendar__day',
      this.props.isWeekend && 'daily-calendar__day--weekend',
      this.props.className
    ]);

    return <td className={className}>
      {this.props.children}
    </td>;
  }
}

interface ICalendarGroupProps {
  group: IActivitiesViewGroup;
  allDays: IExtendendDailyCalendarDay[];
}

function CalendarGroup(props: ICalendarGroupProps) {
  const {name, activities} = props.group;

  return <tbody>
  <tr className="daily-report__group">
    <th>{name}</th>
    <td colSpan={props.allDays.length}></td>
  </tr>
  {activities.map(a => <CalendarActivity
    key={a.name}
    activity={a}
    allDays={props.allDays}/>)}
  </tbody>
}

interface ICalendarActivityProps {
  activity: IActivitiesViewActivity;
  allDays: IExtendendDailyCalendarDay[];
}

function CalendarActivity(props: ICalendarActivityProps) {
  const activityKey = props.activity.activityKey;
  const days = props.allDays.map(day => {
    const activity = day.activities.find(ma => ma.activityKey == activityKey);

    return {
      key: day.year + '-' + day.month,
      isWeekend: day.isWeekend,
      activity: activity
    };
  });

  return <tr className="daily-report__activity">
    <th>{props.activity.name}</th>
    {days.map(m => <CalendarActivityDay key={m.key} isWeekend={m.isWeekend} activity={m.activity}/>)}
  </tr>;
}

interface ICalendarActivityDayProps extends ICalendarDayProps {
  activity?: IActivityOccurrence;
}

function CalendarActivityDay(props: ICalendarActivityDayProps) {
  let { highlighted, missed, ongoing } = props.activity || {
    highlighted: false,
    missed: false,
    ongoing: false
  };

  let className = classNames([
    props.activity && "daily-report__activity-day",
    ongoing && "daily-report__activity-day--ongoing",
    highlighted && "daily-report__activity-day--highlight",
    missed && "daily-report__activity-day--missed"
  ]);
  let children = props.activity && props.activity.note ? "*" : ""

  return <CalendarDay isWeekend={props.isWeekend} className={className}>
    {children}
  </CalendarDay>;
}

