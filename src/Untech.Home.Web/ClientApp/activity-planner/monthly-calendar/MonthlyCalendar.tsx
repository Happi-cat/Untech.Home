import * as React from 'react';
import './MonthlyCalendar.less';
import {IMonthlyCalendar} from '../api';
import {pluralizeMonth} from '../../utils'
import Month from "./Month";
import Group from "./Group";

export interface IMonthlyCalendarProps {
  calendar: IMonthlyCalendar;
}

export class MonthlyCalendar extends React.Component<IMonthlyCalendarProps> {
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
        <th/>
        {headers.map(m => <Month key={m.key} isThisMonth={m.isThisMonth}>
          {m.name}
        </Month>)}
      </tr>
      </thead>

      {groups.map(g => <Group
        key={g.name}
        group={g}
        months={months}
      />)}
    </table>;
  }
}


