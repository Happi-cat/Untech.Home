import * as React from 'react';
import { IMonthlyCalendar } from '../api';

export interface ICalendarProps {
  calendar: IMonthlyCalendar;
}

export class Calendar extends React.Component<ICalendarProps> {
  public render() {
    return <table>

    </table>;
  }
}