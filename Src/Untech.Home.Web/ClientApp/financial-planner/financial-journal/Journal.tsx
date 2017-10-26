import * as React from 'react';
import { IMoney, IFinancialJournalEntry } from '../api/Models';
import { MoneyView, MoneyInput } from '../components';
import { humanizeDate } from '../../utils';

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
        return <table className='table table-bordered'>
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
                <EditRow model={model} onSave={this.handleNewItemSave} onCancel={this.handleNewItemCancel} />
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

    handleNewItemSave = (model: IEditModel) => {
        this.props.onAdd({
            remarks: model.remarks,
            actual: { amount: model.actual, currency: { id: 'BYN' } },
            forecasted: { amount: model.forecasted, currency: { id: 'BYN' } }
        });
    }

    handleNewItemCancel = () => { }
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
            return <EditRow model={model} onSave={this.handleSave} onCancel={this.handleCancel} />;
        }

        return <tr>
            <td>{remarks}</td>
            <td><MoneyView amount={actual.amount} currencyCode={actual.currency.id} /></td>
            <td><MoneyView amount={forecasted.amount} currencyCode={actual.currency.id} /></td>
            <td>{humanizeDate(when)}</td>
            <td>
                {this.props.editable && <button className='btn btn-sm btn-primary' type='button' onClick={this.handleEdit}>Edit</button>}
                <button className='btn btn-sm btn-danger' type='button' onClick={this.handleDelete}>Delete</button>
            </td>
        </tr>;
    }

    handleEdit = () => {
        this.setState({ edit: true });
    }

    handleDelete = () => {
        this.props.onDelete(this.props.model.id);
    }

    handleSave = (model: IEditModel) => {
        this.setState({ edit: false });

        this.props.onUpdate(this.props.model.id, {
            remarks: model.remarks,
            actual: { amount: model.actual, currency: { id: 'BYN' } },
            forecasted: { amount: model.forecasted, currency: { id: 'BYN' } }
        });
    }

    handleCancel = () => {
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
    }

    public render() {
        return <tr>
            <td><input name='remarks' type='text' className='form-control' value={this.state.remarks} onChange={this.handleInputChange} /></td>
            <td>
                <MoneyInput amount={this.state.actual} currencyCode='BYN' onChange={() => { }} />
                <div className='input-group' >
                    <input name='actual' type='number' className='form-control' step='0.01' value={this.state.actual} onChange={this.handleInputChange} />
                    <select className='input-group-addon custom-select'>
                        <option>BYN</option>
                    </select>
                </div>
            </td>
            <td><input name='forecasted' type='number' className='form-control' step='0.01' value={this.state.forecasted} onChange={this.handleInputChange} /></td>
            <td></td>
            <td>
                <button className='btn btn-sm btn-primary' type='button' onClick={this.handleSave}>Save</button>
                <button className='btn btn-sm btn-secondary' type='button' onClick={this.handleCancel}>Cancel</button>
            </td>
        </tr>;
    }

    handleInputChange = (event: any) => {
        const { name, value } = event.target;

        this.setState({ [name]: value });
    }

    handleSave = () => {
        this.props.onSave({
            remarks: this.state.remarks,
            actual: this.state.actual,
            forecasted: this.state.forecasted
        });
    }

    handleCancel = () => {
        this.setState({ remarks: '', actual: 0, forecasted: 0 });
        this.props.onCancel();
    }
}