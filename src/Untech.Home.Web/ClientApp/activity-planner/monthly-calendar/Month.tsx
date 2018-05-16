import * as classNames from "classnames";
import * as React from "react";

export interface IMonthProps {
  isThisMonth: boolean;
  className?: string;
}

export default class Month extends React.PureComponent<IMonthProps> {
  public render() {
    const className = classNames([
      'monthly-calendar__month',
      this.props.isThisMonth && '-this',
      this.props.className
    ]);

    return <td className={className}>
      {this.props.children}
    </td>;
  }
}