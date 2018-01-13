import * as React from 'react';
import { IActivityOccurrence} from '../api'
import { TextArea, Form, Radio, Button } from "semantic-ui-react";
import {pluralizeMonth} from "../../utils";

export interface IOccurrenceEditorProps {
  occurrence: IActivityOccurrence;
  onSubmit(value: IActivityOccurrence): void;
}

export interface IOccurrenceEditorState {
  note: string;
  highlighted: boolean;
  missed: boolean;
}

export class OccurrenceEditor extends React.Component<IOccurrenceEditorProps, IOccurrenceEditorState> {
  constructor(props: any) {
    super(props);

    this.state = {
      note: this.props.occurrence.note || "",
      highlighted: this.props.occurrence.highlighted,
      missed: this.props.occurrence.missed
    };
  }

  public componentWillReceiveProps(nextProps: IOccurrenceEditorProps) {
    this.state = {
      note: nextProps.occurrence.note || "",
      highlighted: nextProps.occurrence.highlighted,
      missed: nextProps.occurrence.missed
    }
  }

  public render() {
    const when = new Date(this.props.occurrence.when);
    const formattedWhen = `${when.getDate()} ${pluralizeMonth(when.getMonth() + 1)} ${when.getFullYear()}`;

    return <Form onSubmit={this.handleSubmit}>
      <Form.Group>
        <label>{formattedWhen}</label>
      </Form.Group>

      <Form.Field
        control={TextArea}
        label='Note'
        placeholder='Leave notes here...'
        onChange={this.handleNoteChange}
      />

      <Form.Group>
        <Radio
          name='highlighted'
          slider
          label='Highlighted'
          defaultChecked={this.props.occurrence.highlighted}
          onChange={this.handleRadioChange}
        />
      </Form.Group>

      <Form.Group>
        <Radio
          name='missed'
          slider
          label='Missed'
          defaultChecked={this.props.occurrence.missed}
          onChange={this.handleRadioChange}
        />
      </Form.Group>

      <Form.Field control={Button}>Submit</Form.Field>
    </Form>
  }

  handleNoteChange = (event: any, data: any) => {
    const {value} = data;
    this.setState({note: value});
  }

  handleRadioChange = (event: any, data: any) => {
    const {name, checked} = data;

    this.setState({[name]: checked});
  }

  handleSubmit = (event: any) => {
    event.preventDefault();

    const value = {
      ...this.props.occurrence,
      note: this.state.note,
      highlighted: this.state.highlighted,
      missed: this.state.missed
    }

    this.props.onSubmit(value);
  }
}