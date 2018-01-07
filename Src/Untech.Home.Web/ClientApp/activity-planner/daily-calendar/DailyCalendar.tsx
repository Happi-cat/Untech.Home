import * as React from 'react';
import * as classNames from 'classnames';
import './DailyCalendar.less';
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

export class DailyCalendar extends React.PureComponent<IDailyCalendarProps> {
  public render() {
    const months = this.props.calendar.months.map(m => ({
      key: m.year + '-' + m.month,
      name: m.year + ' ' + pluralizeMonth(m.month),
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

    return <table className="daily-calendar">
      <thead>
        <tr className="daily-calendar__months">
          <th />
          {months.map(m => <td key={m.key} colSpan={m.daysCount}>{m.name}</td>)}
        </tr>
        <tr className="daily-calendar__days">
          <th />
          {days.map(d => <CalendarDay key={d.key} isWeekend={d.isWeekend} isThisDay={d.isThisDay}>
            {d.day}
          </CalendarDay>)}
        </tr>
        <tr className="daily-calendar__days-of-week">
          <th />
          {days.map(d => <CalendarDay key={d.key} isWeekend={d.isWeekend} isThisDay={d.isThisDay}>
            {pluralizeDayOfWeek(d.dayOfWeek)}
          </CalendarDay>)}
        </tr>
      </thead>
      {groups.map(g => <CalendarGroup
        key={g.name}
        group={g}
        allDays={days}
      />)}
      <tfoot>
        <td></td>
        <td colSpan={days.length} />
      </tfoot>
    </table>;
  }
}

interface IExtendendDailyCalendarDay extends IDailyCalendarDay {
  key: string;
  isWeekend: boolean;
}

interface ICalendarDayProps extends React.TdHTMLAttributes<CalendarDay> {
  isWeekend: boolean;
  isThisDay: boolean;
  className?: string;
}

class CalendarDay extends React.PureComponent<ICalendarDayProps> {
  public render() {
    const className = classNames([
      'daily-calendar__day',
      this.props.isWeekend && 'daily-calendar__day--weekend',
      this.props.isThisDay && 'daily-calendar__day--today',
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
  <tr className="daily-calendar__group">
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
    const occurrence = day.activities.find(ma => ma.activityKey == activityKey);

    return {
      key: day.year + '-' + day.month,
      activityKey: activityKey,
      ...day,
      occurrence: occurrence
    };
  });

  return <tr className="daily-calendar__activity">
    <th>{props.activity.name}</th>
    {days.map(m => <CalendarActivityDay key={m.key} {...m} />)}
  </tr>;
}

interface ICalendarActivityDayProps extends ICalendarDayProps {
  activityKey: number;
  year: number;
  month: number;
  day: number;
  occurrence?: IActivityOccurrence;
}

function CalendarActivityDay(props: ICalendarActivityDayProps) {
  let { highlighted, missed, ongoing } = props.occurrence || {
    highlighted: false,
    missed: false,
    ongoing: false
  };

  let className = classNames([
    props.occurrence && "daily-calendar__activity-day",
    ongoing && "daily-calendar__activity-day--ongoing",
    ongoing && highlighted && "daily-calendar__activity-day--highlight",
    missed && "daily-calendar__activity-day--missed"
  ]);
  let children = props.occurrence && props.occurrence.note ? "*" : ""

  return <CalendarDay isWeekend={props.isWeekend} isThisDay={props.isThisDay} className={className}>
    {children}
  </CalendarDay>;
}

