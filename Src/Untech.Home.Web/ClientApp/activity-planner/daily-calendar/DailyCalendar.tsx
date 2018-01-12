import * as React from 'react';
import * as classNames from 'classnames';
import './DailyCalendar.less';
import {
  IDailyCalendar,
  IDailyCalendarDay,
  IActivitiesViewGroup,
  IActivitiesViewActivity, IActivityOccurrence
} from '../api';
import { pluralizeDayOfWeek, pluralizeMonth } from '../../utils'
import { QuickAdder } from "../components/QuickAdder";
import { SmartQuickEditor } from "../components/SmartQuickEditor";
import { Button } from "semantic-ui-react";

export interface IDailyCalendarProps {
  calendar: IDailyCalendar;
  dispatcher: IDailyCalendarDispatcher;
}

export interface IDailyCalendarDispatcher {
  onAddGroup(name: string): void;

  onDeleteGroup(id: number): void;

  onAddActivity(groupId: number, name: string): void;

  onUpdateActivity(id: number, name: string): void;

  onDelteActivity(id: number): void;

  onToggleActivityOccurrence(activityId: number, year: number, month: number, day: number): void;

  onActivityOccurrenceSelected(occurrence: IActivityOccurrence): void;
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
          <th />
          {months.map(m => <td key={m.key} colSpan={m.daysCount}>{m.name}</td>)}
        </tr>
        <tr className="daily-calendar__days">
          <th />
          <th />
          {days.map(d => <CalendarDay key={d.key} isWeekend={d.isWeekend} isThisDay={d.isThisDay}>
            {d.day}
          </CalendarDay>)}
        </tr>
        <tr className="daily-calendar__days-of-week">
          <th />
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
        dispatcher={this.props.dispatcher}
      />)}
      <tfoot>
        <td />
        <td>
          <QuickAdder onSave={this.props.dispatcher.onAddGroup} placeholder="Add group..." />
        </td>
        <td colSpan={days.length} />
      </tfoot>
    </table>;
  }
}

interface IExtendendDailyCalendarDay extends IDailyCalendarDay {
  key: string;
  isWeekend: boolean;
}

interface ICalendarDayProps extends React.TdHTMLAttributes<HTMLTableDataCellElement> {
  isWeekend: boolean;
  isThisDay: boolean;
  className?: string;
}

class CalendarDay extends React.PureComponent<ICalendarDayProps> {
  public render() {
    const { isWeekend, isThisDay, className, ...other } = this.props;
    const elementClassName = classNames([
      'daily-calendar__day',
      isWeekend && 'daily-calendar__day--weekend',
      isThisDay && 'daily-calendar__day--today',
      className
    ]);

    return <td className={elementClassName} {...other}>
      {this.props.children}
    </td>;
  }
}

interface ICalendarGroupProps {
  group: IActivitiesViewGroup;
  allDays: IExtendendDailyCalendarDay[];
  dispatcher: IDailyCalendarDispatcher;
}

interface ICalendarGroupState {
  expanded: boolean;
}

class CalendarGroup extends React.Component<ICalendarGroupProps, ICalendarGroupState> {
  constructor(props: any) {
    super(props);

    this.state = { expanded: false };
  }

  public render() {
    const { name, activities } = this.props.group;

    let icon = this.state.expanded
      ? 'triangle down'
      : 'triangle right';

    return <tbody>
      <tr className="daily-calendar__group">
        <th>
          <Button size='mini' icon={icon} onClick={this.toggleExpanded} />
        </th>
        <th>
          <SmartQuickEditor value={name} onSave={this.handleGroupSave} onDelete={this.handleGroupDelete} />
        </th>
        <td colSpan={this.props.allDays.length} />
      </tr>
      {this.state.expanded && activities.map(a => <CalendarActivity
        key={a.name}
        activity={a}
        allDays={this.props.allDays}
        dispatcher={this.props.dispatcher} />)}
      {this.state.expanded && <tr>
        <td />
        <td><QuickAdder onSave={this.handleActivityAdd} placeholder="Add activity..." /></td>
        <td colSpan={this.props.allDays.length} />
      </tr>}
    </tbody>
  }

  handleGroupSave = () => {
  };
  handleGroupDelete = () => {
    this.props.dispatcher.onDeleteGroup(this.props.group.groupKey);
  }
  handleActivityAdd = (name: string) => {
    this.props.dispatcher.onAddActivity(this.props.group.groupKey, name);
  }
  toggleExpanded = () => {
    this.setState(function (prevState, props) {
      const expanded = prevState.expanded;
      return { expanded: !expanded };
    });
  }
}

interface ICalendarActivityProps {
  activity: IActivitiesViewActivity;
  allDays: IExtendendDailyCalendarDay[];
  dispatcher: IDailyCalendarDispatcher;
}

class CalendarActivity extends React.PureComponent<ICalendarActivityProps> {
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
      <th />
      <th>
        <SmartQuickEditor value={this.props.activity.name} onSave={this.handleSave} onDelete={this.handleDelete} />
      </th>
      {days.map(m => <CalendarActivityDay key={m.key} dispatcher={this.props.dispatcher} {...m} />)}
    </tr>;
  }

  handleSave = (name: string) => {
    this.props.dispatcher.onUpdateActivity(this.props.activity.activityKey, name);
  }

  handleDelete = () => {
    this.props.dispatcher.onDelteActivity(this.props.activity.activityKey);
  }
}

interface ICalendarActivityDayProps extends ICalendarDayProps {
  activityKey: number;
  year: number;
  month: number;
  day: number;
  occurrence?: IActivityOccurrence;
  dispatcher: IDailyCalendarDispatcher;
}

class CalendarActivityDay extends React.PureComponent<ICalendarActivityDayProps> {
  public render() {
    let { highlighted, missed, ongoing } = this.props.occurrence || {
      highlighted: false,
      missed: false,
      ongoing: false
    };

    let className = classNames([
      this.props.occurrence && "daily-calendar__activity-day",
      ongoing && "daily-calendar__activity-day--ongoing",
      ongoing && highlighted && "daily-calendar__activity-day--highlight",
      missed && "daily-calendar__activity-day--missed"
    ]);
    let children = this.props.occurrence && this.props.occurrence.note ? "*" : ""

    return <CalendarDay
      isWeekend={this.props.isWeekend}
      isThisDay={this.props.isThisDay}
      className={className}
      onClick={this.handleClick}
    >
      {children}
    </CalendarDay>;
  }

  handleClick = (event: any) => {
    if (event.ctrlKey) {
      const {activityKey, year, month, day} = this.props;
      this.props.dispatcher.onToggleActivityOccurrence(activityKey, year, month, day);
    } else {
      if (this.props.occurrence) {
        this.props.dispatcher.onActivityOccurrenceSelected(this.props.occurrence);
      }
    }
  }
}
