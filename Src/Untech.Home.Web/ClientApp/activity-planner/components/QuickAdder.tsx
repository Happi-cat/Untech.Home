import * as React from 'react';
import {Input, Button} from "semantic-ui-react";

export interface IQuickAdderProps {
  placeholder?: string;

  onSave(value: string): void;
}

export interface IQuickAdderState {
  value: string
}

export class QuickAdder extends React.Component<IQuickAdderProps, IQuickAdderState> {
  constructor(props: any) {
    super(props);

    this.state = {value: ""};
  }

  public render() {
    return <div>
      <Input
        fluid
        name='value'
        size='mini'
        type='text'
        defaultValue={this.state.value}
        onChange={this.handleInputChange}
        placeholder={this.props.placeholder}
        action={<Button.Group floated='right' size='mini'>
          <Button onClick={this.handleSave} icon='add' positive/>
        </Button.Group>}
      />
    </div>;
  }

  handleInputChange = (event: any, data: any) => {
    const {name, value} = data;

    this.setState({[name]: value});
  }

  handleSave = () => {
    this.props.onSave(this.state.value);
  }
}