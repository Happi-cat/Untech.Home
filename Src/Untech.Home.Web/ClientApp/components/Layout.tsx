import * as React from 'react';
import { NavMenu } from './NavMenu';

export interface LayoutProps {
    children?: React.ReactNode;
}

export class Layout extends React.Component<LayoutProps, {}> {
    public render() {
        return <div>
            <NavMenu />
            <div style={{ marginLeft: '15rem' }} >
                {this.props.children}
            </div>
        </div>;
    }
}
