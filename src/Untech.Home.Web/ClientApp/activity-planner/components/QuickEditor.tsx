import * as React from 'react';
import {Input, Button} from "semantic-ui-react";

export interface IQuickEditorProps {
  placeholder?:string;
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
      <Input
        fluid
        name='value'
        size='mini'
        type='text'
        defaultValue={this.state.value}
        placeholder={this.props.placeholder}
        onChange={this.handleInputChange}
        action={<Button.Group floated='right' size='mini'>
          <Button onClick={this.handleCancel} icon='cancel'/>
          <Button onClick={this.handleSave} icon='save' positive/>
        </Button.Group>}
      />


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