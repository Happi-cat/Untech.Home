import * as React from 'react';
import {Input, Button, Loader} from "semantic-ui-react";

export interface IQuickEditorProps {
  placeholder?:string;
  value: string;
  onSave(value: string) : Promise<void>;
  onCancel() : void;
}

export interface IQuickEditorState {
  value: string,
  isSaving: boolean,
}

export class QuickEditor extends React.Component<IQuickEditorProps, IQuickEditorState> {
  constructor(props: any) {
    super(props);

    this.state = { value: props.value, isSaving: false };
  }

  public render() {
    if (this.state.isSaving)
    {
      return <div>
        <Loader active inline size={"small"}  >
          Saving
        </Loader>
      </div>
    }

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

  handleSave = async () => {
    this.setState({ isSaving: true });
    await this.props.onSave(this.state.value);
    this.setState({ isSaving: false });
  }

  handleCancel = () => {
    this.props.onCancel();
  }
}