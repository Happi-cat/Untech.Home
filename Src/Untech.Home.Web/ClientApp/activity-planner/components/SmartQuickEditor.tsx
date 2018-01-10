import * as React from 'react';
import {QuickEditor} from "./QuickEditor";
import {Button} from "semantic-ui-react";

export interface ISmartQuickEditorProps {
  value: string;
  onSave(value: string) : void;
  onDelete() : void;
}

export interface ISmartQuickEditorState {
  editMode: boolean;
}

export class SmartQuickEditor extends React.Component<ISmartQuickEditorProps, ISmartQuickEditorState> {
  constructor(props: any) {
    super(props);

    this.state = { editMode: false };
  }

  public render() {
    if (this.state.editMode) {
      return <div>
        <QuickEditor value={this.props.value} onSave={this.props.onSave} onCancel={this.handleCancel}/>
      </div>
    }

    return <div>
      <div>{this.props.value}</div>
      <Button.Group floated='right' size='mini'>
        <Button onClick={this.props.onDelete} icon='delete' content='Delete' />
        <Button onClick={this.handleEdit} icon='edit' content='Edit' positive/>
      </Button.Group>
    </div>
  }

  handleEdit = () => {
    this.setState({ editMode: true });
  }
  handleCancel = () => {
    this.setState({ editMode: false });
  }
}
