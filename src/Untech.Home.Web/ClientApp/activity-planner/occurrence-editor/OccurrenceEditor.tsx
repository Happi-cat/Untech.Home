import * as React from 'react';
import {IActivityOccurrence} from '../api'
import {TextArea, Form, Radio, Button} from "../components";
import {pluralizeMonth} from "../../utils";
import {updateActivityOccurrence} from "../actions";
import {connect} from "react-redux";
import {State} from "../types";

export interface IOccurrenceEditorProps {
  occurrence: IActivityOccurrence;
  onSubmit: typeof updateActivityOccurrence
}

export interface IOccurrenceEditorState {
  note: string;
  highlighted: boolean;
  missed: boolean;
}

class OccurrenceEditor extends React.Component<IOccurrenceEditorProps, IOccurrenceEditorState> {
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

    this.props.onSubmit({
      ...this.props.occurrence,
      note: this.state.note,
      highlighted: this.state.highlighted,
      missed: this.state.missed
    });
  }
}

const mapStateToProps = (state: State) => ({
  occurrence: state.selectedActivityOccurrence || {}
})

const mapDispatchToProps = {
  onSubmit: updateActivityOccurrence
}

export default connect(mapStateToProps, mapDispatchToProps)(OccurrenceEditor)