import * as React from "react";
import * as classNames from "classnames";
import Month from "./Month";

export interface IActivityMonthProps {
  count: number,
  isThisMonth: boolean
}

export default class ActivityMonth extends React.PureComponent<IActivityMonthProps> {
  public render() {
    const {count, isThisMonth} = this.props;
    const level = count
      ? Math.floor(Math.log2(count)) + 1
      : 0;

    const className = classNames([
      level && ('-level-' + level)
    ]);

    return <Month isThisMonth={isThisMonth} className={className}>
      {count ? count : null}
    </Month>;
  }
}