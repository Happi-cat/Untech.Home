import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';
import { Menu, Icon } from 'semantic-ui-react';

export class NavMenu extends React.Component {
    public render() {
        return <Menu vertical fixed='left' className='menu--scrollable'>
            <Menu.Item as={NavLink} to={'/'} exact activeClassName='active' >
                <Icon name='home' />
                Home
            </Menu.Item>

            <Menu.Item as={NavLink} to='/activity-planner' activeClassName='active' >
                <Icon name='bicycle' />
                Activity planner
            </Menu.Item>

            <Menu.Item as={NavLink} to='/financial-planner' activeClassName='active' >
                <Icon name='dollar' />
                Financial planner
            </Menu.Item>
        </Menu>;
    }
}
