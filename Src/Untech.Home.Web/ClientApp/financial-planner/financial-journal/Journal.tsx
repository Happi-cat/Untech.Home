import * as React from 'react';
import { IMoney, IFinancialJournalEntry } from '../api/Models';
import Money from '../components/Money';

export interface IFinancialJournalEntryChange {
    remarks: string;
    actual?: IMoney;
    forecasted?: IMoney;
}

interface IJournalProps {
    entries: IFinancialJournalEntry[],
    editable: boolean;
    onAdd(args: IFinancialJournalEntryChange): void;
    onUpdate(id: number, args: IFinancialJournalEntryChange): void;
    onDelete(id: number): void;
}

export class Journal extends React.Component<IJournalProps> {
    public render() {
        const model = { remarks: '', actual: 0, forecasted: 0 };
        return <table >
            <thead>
                <tr>
                    <th>Remarks</th>
                    <th>Actual</th>
                    <th>Forecasted</th>
                    <th>When</th>
                    <th>Actions</th>
                </tr>
            </thead>

            {this.props.editable && <tbody>
                <EditRow model={model} onSave={this.onNewItemSave} onCancel={this.onNewItemCancel} />
            </tbody>}

            <tbody>
                {this.props.entries.map(e => <Row key={e.id}
                    model={e}
                    editable={this.props.editable}
                    onUpdate={this.props.onUpdate}
                    onDelete={this.props.onDelete} />)}
            </tbody>
        </table>;
    }

    onNewItemSave = (model: IEditModel) => {
        this.props.onAdd({
            remarks: model.remarks,
            actual: { amount: model.actual, currency: { id: 'BYN' } },
            forecasted: { amount: model.forecasted, currency: { id: 'BYN' } }
        });
    }

    onNewItemCancel = () => { }
}

interface IRowProps {
    model: IFinancialJournalEntry,
    editable: boolean,
    onUpdate(id: number, args: IFinancialJournalEntryChange): void;
    onDelete(id: number): void;
}

interface IRowState {
    edit: boolean
}

class Row extends React.Component<IRowProps, IRowState> {
    constructor(props: any) {
        super(props);

        this.state = { edit: false };
    }

    public render() {
        const { remarks, actual, forecasted, when } = this.props.model;

        if (this.state.edit) {
            const model = {
                remarks: remarks,
                actual: actual.amount,
                forecasted: forecasted.amount
            };
            return <EditRow model={model} onSave={this.onSave} onCancel={this.onCancel} />;
        }

        return <tr>
            <td>{remarks}</td>
            <td><Money amount={actual.amount} currencyCode={actual.currency.id} /></td>
            <td><Money amount={forecasted.amount} currencyCode={actual.currency.id} /></td>
            <td>{when}</td>
            <td>
                {this.props.editable && <button onClick={this.onEdit}>Edit</button>}
                <button onClick={this.onDelete}>Delete</button>
            </td>
        </tr>;
    }

    onEdit = () => {
        this.setState({ edit: true });
    }

    onDelete = () => {
        this.props.onDelete(this.props.model.id);
    }

    onSave = (model: IEditModel) => {
        this.setState({ edit: false });

        this.props.onUpdate(this.props.model.id, {
            remarks: model.remarks,
            actual: { amount: model.actual, currency: { id: 'BYN' } },
            forecasted: { amount: model.forecasted, currency: { id: 'BYN' } }
        });
    }

    onCancel = () => {
        this.setState({ edit: false });
    }
}

interface IEditModel {
    remarks: string;
    actual: number;
    forecasted: number;
}

interface IEditRowProps {
    model: IEditModel;
    onSave(model: IEditModel): void;
    onCancel(): void;
}

interface IEditRowState {
    remarks: string;
    actual: number;
    forecasted: number;
}

class EditRow extends React.Component<IEditRowProps, IEditRowState> {
    constructor(props: any) {
        super(props);

        const model = this.props.model;
        this.state = {
            remarks: model.remarks,
            actual: model.actual,
            forecasted: model.forecasted
        };

        this.onInputChange = this.onInputChange.bind(this);
        this.onSave = this.onSave.bind(this);
        this.onCancel = this.onCancel.bind(this);
    }

    public render() {
        return <tr>
            <td><input name='remarks' type='text' value={this.state.remarks} onChange={this.onInputChange} /></td>
            <td><input name='actual' type='number' step='0.01' value={this.state.actual} onChange={this.onInputChange} /></td>
            <td><input name='forecasted' type='text' value={this.state.forecasted} onChange={this.onInputChange} /></td>
            <td></td>
            <td>
                <button onClick={this.onSave}>Save</button>
                <button onClick={this.onCancel}>Cancel</button>
            </td>
        </tr>;
    }

    onInputChange(event: any) {
        const { name, value } = event.target;

        this.setState({ [name]: value });
    }

    onSave() {
        this.props.onSave({
            remarks: this.state.remarks,
            actual: this.state.actual,
            forecasted: this.state.forecasted
        });
    }

    onCancel() {
        this.setState({ remarks: '', actual: 0, forecasted: 0 });
        this.props.onCancel();
    }
}