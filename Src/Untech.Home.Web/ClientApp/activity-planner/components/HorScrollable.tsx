import * as React from 'react';
import './HorScrollable.less';

export class HorScrollable extends React.PureComponent {
  public render() {
    return <div className='hor-scrollable'>
      {this.props.children}
    </div>
  }
}