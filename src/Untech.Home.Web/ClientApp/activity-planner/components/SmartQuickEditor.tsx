import * as React from 'react';
import { QuickEditor } from "./QuickEditor";
import { Button, Loader } from "semantic-ui-react";
import './SmartQuickEditor.less';


export interface ISmartQuickEditorProps {
  value: string;
  onSave(value: string): Promise<void>;
  onDelete(): Promise<void>;
}

export interface ISmartQuickEditorState {
  editMode: boolean;
  isDeleting: boolean;
}

export class SmartQuickEditor extends React.Component<ISmartQuickEditorProps, ISmartQuickEditorState> {
  constructor(props: any) {
    super(props);

    this.state = { editMode: false, isDeleting: false };
  }

  public render() {
    if (this.state.editMode) {
      return <div>
        <QuickEditor value={this.props.value} onSave={this.props.onSave} onCancel={this.handleCancel} />
      </div>
    }

    if (this.state.isDeleting)
    {
      return <div>
        <Loader active inline size={"small"}  >
          Deleting
        </Loader>
      </div>
    }

    return <div className="smart-quick-editor">
      <div className="smart-quick-editor__label">{this.props.value}</div>
      <Button.Group floated='right' size='mini'>
        <Button onClick={this.handleDelete} icon='trash' color='orange' />
        <Button onClick={this.handleEdit} icon='edit' positive />
      </Button.Group>
    </div>
  }

  handleDelete = async () => {
    this.setState({ isDeleting: true });
    await this.props.onDelete();
    this.setState({ isDeleting: false });
  }

  handleEdit = () => {
    this.setState({ editMode: true });
  }
  handleCancel = () => {
    this.setState({ editMode: false });
  }
}
