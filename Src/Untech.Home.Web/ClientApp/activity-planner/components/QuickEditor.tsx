import * as React from 'react';
import {Input, Button} from "semantic-ui-react";

export interface IQuickEditorProps {
  value: string;
  onSave(value: string) : void;
  onCancel() : void;
}

export interface IQuickEditorState {
  value: string
}

export class QuickEditor extends React.Component<IQuickEditorProps, IQuickEditorState> {
  constructor(props: any) {
    super(props);

    this.state = { value: props.value };
  }

  public render() {
    return <div>
      <Input fluid name='value' size='mini' type='text' defaultValue={this.state.value} onChange={this.handleInputChange}/>

      <Button.Group floated='right' size='mini'>
        <Button onClick={this.handleCancel} icon='cancel' content='Cancel'/>
        <Button onClick={this.handleSave} icon='save' content='Save' positive/>
      </Button.Group>
    </div>;
  }

  handleInputChange = (event: any, data: any) => {
    const { name, value } = data;

    this.setState({ [name]: value });
  }

  handleSave = () => {
    this.props.onSave(this.state.value);
  }

  handleCancel = () => {
    this.props.onCancel();
  }
}