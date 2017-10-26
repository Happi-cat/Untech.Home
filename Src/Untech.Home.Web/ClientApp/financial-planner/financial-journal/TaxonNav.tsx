import * as React from 'react';
import { ITaxonTree } from '../api/Models';

interface ITaxonNavProps {
    model: ITaxonTree,
    onClick(taxonId: number): void;
}

export class TaxonNav extends React.Component<ITaxonNavProps, {}> {
    public render() {
        const upVisible = this.props.model.id != 0;

        let elements = this.props.model.elements || [];
        return <nav>
            <ul className='pagination' >
                {upVisible && <li className='page-item' onClick={this.onUpClick} >
                    <span className='page-link'>Up</span>
                </li>}
                <li className='page-item active' >
                    <span className='page-link'>{this.props.model.name}</span>
                </li>
                {elements.map(e => <TaxonNavItem key={e.id} model={e} onClick={this.props.onClick} />)}
            </ul>
        </nav>;
    }

    onUpClick = () => {
        this.props.onClick(this.props.model.parentId);
    }
}

class TaxonNavItem extends React.Component<ITaxonNavProps> {
    public render() {
        return <li className='page-item' onClick={this.handleClick}><span className='page-link'>{this.props.model.name}</span></li>;
    }

    handleClick = () => {
        this.props.onClick(this.props.model.id);
    }
}