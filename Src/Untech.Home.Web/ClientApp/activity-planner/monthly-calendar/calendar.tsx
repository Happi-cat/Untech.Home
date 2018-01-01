import * as React from 'react';
import { IMonthlyCalendar, IActivitiesViewActivity, IMonthlyCalendarMonthActivity } from '../api';
import { activitiesViewToDict } from '../utils';
import { pluralizeMonth } from '../../utils'

export interface ICalendarProps {
  calendar: IMonthlyCalendar;
}

export class Calendar extends React.Component<ICalendarProps> {
  public render() {
    const months = this.props.calendar.months
      .map(n => n.month)
      .map(pluralizeMonth);

    return <table>
      <thead>
        <tr>
          <th></th>
          {months.map(m => <th key={m}>{m}</th>)}
        </tr>
      </thead>
      <tbody>
      
      </tbody>
    </table>;
  }
}

interface IActivity {
  key: number;
  name: string;
  months: IActivityMonth[];
}

interface IActivityMonth {
  year: number;
  month: number;
  count: number;
}

function transform(request: IMonthlyCalendar) {
  const dict = activitiesViewToDict(request.view);

  
}

interface IActivityProps {
  activity: IActivitiesViewActivity;
  months: IActivityMonthProps[];
}

class Activity extends React.Component<IActivityProps> {
  public render() {
    return <tr>
      <td>{this.props.activity.name}</td>

    </tr>;
  }
}

interface IActivityMonthProps {
  count: number;
}

class ActivityMonth extends React.Component<IActivityMonthProps> {
  public render() {
    return <td>
      {this.props.count}
    </td>;
  }
}
