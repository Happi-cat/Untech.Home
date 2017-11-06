import * as React from 'react';
import * as classNames from 'classnames';
import { pluralizeMonth } from '../../utils';
import './MonthView.less';

interface IMonthViewProps {
  year: number;
  month: number;
  className?: string;
}

export function MonthView(props: IMonthViewProps) {
  const cls = classNames('month-view', props.className);

  return <div className={cls}>
    <div className='month-view__month'>{pluralizeMonth(props.month)}</div>
    <div className='month-view__year'>{props.year}</div>
  </div>;
}

export default MonthView;

