import * as React from "react";
import * as classNames from "classnames";
import {default as Day, IDayProps} from "./Day";
import {Popup, Divider} from "../components";
import {IActivityOccurrence} from "../api";
import {
  selectActivityOccurrence,
  toggleActivityOccurrence,
} from "../actions";
import {connect} from "react-redux";

export interface IActivityDayProps extends IDayProps {
  activityKey: number;
  year: number;
  month: number;
  day: number;
  occurrence?: IActivityOccurrence;

  onToggleActivityOccurrence: typeof toggleActivityOccurrence;
  onSelectActivityOccurrence: typeof selectActivityOccurrence;
}

class ActivityDay extends React.PureComponent<IActivityDayProps> {
  public render() {
    const {highlighted, missed, ongoing, note} = this.props.occurrence || {
      highlighted: false,
      missed: false,
      ongoing: false,
      note: ''
    };

    const className = classNames([
      this.props.occurrence && "daily-calendar__activity-day",
      ongoing && "-ongoing",
      ongoing && highlighted && "-highlight",
      missed && "-missed"
    ]);

    return <Popup
      content={
        <Tooltip text={note}/>
      }
      inverted
      trigger={
        <Day
          isWeekend={this.props.isWeekend}
          isThisDay={this.props.isThisDay}
          className={className}
          onClick={this.handleClick}
        >
          {note && <Marker/>}
        </Day>}
    />;
  }

  handleClick = (event: any) => {
    if (event.ctrlKey) {
      const {activityKey, year, month, day} = this.props;
      this.props.onToggleActivityOccurrence(activityKey, year, month, day);
    } else {
      if (this.props.occurrence) {
        this.props.onSelectActivityOccurrence(this.props.occurrence);
      }
    }
  };
}

const mapDispatchToProps = {
  onToggleActivityOccurrence: toggleActivityOccurrence,
  onSelectActivityOccurrence: selectActivityOccurrence
}

export default connect(() => ({}), mapDispatchToProps)(ActivityDay);

function Tooltip({text}: { text?: string }) {
  const paragraphs = (text || '')
    .split('\n')
    .filter(n => n);

  const showNotes = !!paragraphs.length;

  return <div>
    <div>
      <b>Click</b> - for edit;
      <br/>
      <b>Ctrl-click</b> - for toggle.
    </div>

    {showNotes && <Divider inverted horizontal>Note</Divider>}

    {paragraphs.map((v, i) => <div key={v + i}>
      {v}
    </div>)}
  </div>;
}

function Marker() {
  return <div className='daily-calendar__activity-day-marker'></div>;
}