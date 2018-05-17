import * as React from 'react';
import './DailyCalendar.less';
import {IDailyCalendar} from '../api';
import {pluralizeDayOfWeek, pluralizeMonth} from '../../utils'
import {QuickAdder} from "../components";
import {connect} from "react-redux";
import {addGroup} from "../actions";
import Group from "./Group";
import Day from "./Day";

export interface IDailyCalendarProps {
  calendar: IDailyCalendar;

  onAddGroup: typeof addGroup;
}

class DailyCalendar extends React.PureComponent<IDailyCalendarProps> {
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
        <th/>
        <th/>
        {months.map(m => <td className="daily-calendar__month" key={m.key} colSpan={m.daysCount}>{m.name}</td>)}
      </tr>

      <tr className="daily-calendar__days">
        <th/>
        <th/>
        {days.map(d => <Day key={d.key} isWeekend={d.isWeekend} isThisDay={d.isThisDay}>
          {d.day}
        </Day>)}
      </tr>

      <tr className="daily-calendar__days-of-week">
        <th/>
        <th/>
        {days.map(d => <Day key={d.key} isWeekend={d.isWeekend} isThisDay={d.isThisDay}>
          {pluralizeDayOfWeek(d.dayOfWeek)}
        </Day>)}
      </tr>
      </thead>

      {groups.map(g => <Group
        key={g.name}
        group={g}
        allDays={days}
      />)}

      <tfoot>
      <tr>
        <td/>
        <td>
          <QuickAdder onSave={this.handleAddGroup} placeholder="Add group..."/>
        </td>
        <td colSpan={days.length}/>
      </tr>
      </tfoot>
    </table>;
  }

  handleAddGroup = async (value: string) => {
    await this.props.onAddGroup(value);
  }
}

const mapDispatchToProps = {
  onAddGroup: addGroup,
}

export default connect(() => ({}), mapDispatchToProps)(DailyCalendar)

