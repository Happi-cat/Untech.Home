import * as React from 'react';
import { ITaxonTree } from '../api/Models';

interface ITaxonNavProps {
    model: ITaxonTree,
    onClick(taxonId: number): void;
}

export class TaxonNav extends React.Component<ITaxonNavProps, {}> {
    constructor(props: any) {
        super(props);

        this.onUpClick = this.onUpClick.bind(this);
    }

    public render() {
        const upVisible = this.props.model.id != 0;

        let upLink = upVisible
            ? <button onClick={this.onUpClick}>Up</button>
            : null;

        let elements = this.props.model.elements || [];
        return <div>
            {upLink}

            {elements.map(e => <TaxonNavItem key={e.id} model={e} onClick={this.props.onClick} />)}
        </div>;
    }

    onUpClick() {
        this.props.onClick(this.props.model.parentId);
    }
}

class TaxonNavItem extends React.Component<ITaxonNavProps> {
    constructor(props: any) {
        super(props);

        this.onClick = this.onClick.bind(this);
    }

    public render() {
        return <button onClick={this.onClick}>
            {this.props.model.name}
        </button>;
    }

    onClick() {
        this.props.onClick(this.props.model.id);
    }
}