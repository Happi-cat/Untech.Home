import * as React from "react";
import {IActivitiesViewGroup} from "../api";
import {QuickAdder} from "../components";
import {Button} from "semantic-ui-react";
import {default as Activity, IActivityDay} from "./Activity";
import {SmartQuickEditor} from "../components";
import {addActivity, deleteGroup, updateGroup} from "../actions";
import {connect} from "react-redux";

interface IGroupProps {
  group: IActivitiesViewGroup;
  allDays: IActivityDay[];

  onUpdateGroup: typeof updateGroup;
  onDeleteGroup: typeof deleteGroup;
  onAddActivity: typeof addActivity;
}

interface IGroupState {
  expanded: boolean;
}

class Group extends React.Component<IGroupProps, IGroupState> {
  constructor(props: any) {
    super(props);

    this.state = {expanded: false};
  }

  public render() {
    const {name, activities} = this.props.group;

    return <tbody>
    <tr className="daily-calendar__group">
      <th>
        <Button size='mini' icon={this.expandIcon} onClick={this.handleToggleExpanded}/>
      </th>
      <th>
        <SmartQuickEditor value={name} onSave={this.handleGroupSave} onDelete={this.handleGroupDelete}/>
      </th>

      <td colSpan={this.daysCount}/>
    </tr>

    {this.state.expanded && activities.map(a => <Activity
      key={a.name}
      activity={a}
      allDays={this.props.allDays}
    />)}

    {this.state.expanded && <tr>
      <td/>
      <td><QuickAdder onSave={this.handleActivityAdd} placeholder="Add activity..."/></td>
      <td colSpan={this.daysCount}/>
    </tr>}
    </tbody>
  }

  get expandIcon() {
    return this.state.expanded
      ? 'triangle down'
      : 'triangle right';
  }

  get daysCount() {
      return this.props.allDays.length
  }

  handleGroupSave = async (name: string) => {
    this.props.onUpdateGroup(this.props.group.groupKey, name);
  };
  handleGroupDelete = async () => {
    this.props.onDeleteGroup(this.props.group.groupKey);
  }
  handleActivityAdd = async (name: string) => {
    this.props.onAddActivity(this.props.group.groupKey, name);
  }
  handleToggleExpanded = () => {
    this.setState(function (prevState, props) {
      return {expanded: !prevState.expanded};
    });
  }
}

const mapDispatchToProps = {
  onUpdateGroup: updateGroup,
  onDeleteGroup: deleteGroup,
  onAddActivity: addActivity
}

export default connect(() => ({}), mapDispatchToProps)(Group);