import * as React from "react";
import * as classNames from "classnames";

export interface IDayProps extends React.TdHTMLAttributes<HTMLTableDataCellElement> {
  isWeekend: boolean;
  isThisDay: boolean;
  className?: string;
}

export default class Day extends React.PureComponent<IDayProps> {
  public render() {
    const {isWeekend, isThisDay, className, ...other} = this.props;
    const elementClassName = classNames([
      'daily-calendar__day', {
        '-weekend': isWeekend,
        '-today': isThisDay
      },
      className
    ]);

    return <td className={elementClassName} {...other}>
      {this.props.children}
    </td>;
  }
}