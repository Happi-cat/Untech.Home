import * as React from 'react';
import {Input, Button} from "semantic-ui-react";


export class NameEditor extends React.Component {
  public render() {
    return <div>
      <Input fluid name='remarks' type='text' defaultValue={this.state.remarks} onChange={this.handleInputChange}/>
      <Button.Group
        floated='right'>
        <Button onClick={this.handleCancel} icon='cancel' content='Cancel'/>
        <Button onClick={this.handleSave} icon='save' content='Save' positive/>
      </Button.Group>
    </div>;
  }
}