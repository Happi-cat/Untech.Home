import * as React from 'react';
import { IMoney, IFinancialJournalEntry, apiService } from '../api';
import { MoneyView, MoneyInput } from '../components';
import { Input, Button, Icon, Table } from 'semantic-ui-react';
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

export class Journal extends React.Component<IJournalProps, { addNew: boolean }> {
    constructor(props: any) {
        super(props);

        this.state = { addNew: false };
    }

    public render() {
        const model = { remarks: '', actual: 0, actualCurrency: 'BYN', forecasted: 0, forecastedCurrency: 'BYN' };

        return <Table>
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
                {(this.state.addNew) ?
                    <EditRow model={model} onSave={this.handleNewItemSave} onCancel={this.handleNewItemCancel} />
                    : <tr>
                        <th></th>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td>
                            <Button.Group floated='right'>
                                <Button onClick={this.handleNewItem} icon='plus' content='New' positive />
                            </Button.Group>
                        </td>
                    </tr>}
            </tbody>}

            <tbody>
                {this.props.entries.map(e => <Row key={e.key}
                    model={e}
                    editable={this.props.editable}
                    onUpdate={this.props.onUpdate}
                    onDelete={this.props.onDelete} />
                )}
            </tbody>
        </Table>;
    }

    handleNewItem = () => {
        this.setState({ addNew: true });
    }

    handleNewItemSave = (model: IEditModel) => {
        this.props.onAdd({
            remarks: model.remarks,
            actual: { amount: model.actual, currency: { id: 'BYN' } },
            forecasted: { amount: model.forecasted, currency: { id: 'BYN' } }
        });

        this.setState({ addNew: false });
    }

    handleNewItemCancel = () => {
        this.setState({ addNew: false });
    }
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
                actualCurrency: actual.currency.id,
                forecasted: forecasted.amount,
                forecastedCurrency: forecasted.currency.id
            };
            return <EditRow model={model} onSave={this.handleSave} onCancel={this.handleCancel} />;
        }

        return <tr>
            <td>{remarks}</td>
            <td><MoneyView amount={actual.amount} currencyCode={actual.currency.id} /></td>
            <td><MoneyView amount={forecasted.amount} currencyCode={actual.currency.id} /></td>
            <td>{humanizeDate(when)}</td>
            <td>
                <Button.Group floated='right'>
                    {this.props.editable &&
                        <Button onClick={this.handleEdit} icon='edit' content='Edit' primary />
                    }
                    <Button onClick={this.handleDelete} icon='delete' content='Delete' color='orange' />
                </Button.Group>
            </td>
        </tr>;
    }

    handleEdit = () => {
        this.setState({ edit: true });
    }

    handleDelete = () => {
        this.props.onDelete(this.props.model.key);
    }

    handleSave = (model: IEditModel) => {
        this.setState({ edit: false });

        this.props.onUpdate(this.props.model.key, {
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
    actualCurrency: string;
    forecasted: number;
    forecastedCurrency: string;
}

interface IEditRowProps {
    model: IEditModel;
    onSave(model: IEditModel): void;
    onCancel(): void;
}

interface IEditRowState {
    remarks: string;
    actual: number;
    actualCurrency: string;
    forecasted: number;
    forecastedCurrency: string;
}

class EditRow extends React.Component<IEditRowProps, IEditRowState> {
    constructor(props: any) {
        super(props);

        const model = this.props.model;
        this.state = { ...model };
    }

    public render() {
        return <tr>
            <td>
                <Input fluid name='remarks' type='text' defaultValue={this.state.remarks} onChange={this.handleInputChange} /></td>
            <td>
                <MoneyInput fluid name='actual' initialAmount={this.state.actual} initialCurrencyCode={this.state.actualCurrency} onChange={this.handleMoneyChange} />
            </td>
            <td>
                <MoneyInput fluid name='forecasted' initialAmount={this.state.forecasted} initialCurrencyCode={this.state.forecastedCurrency} onChange={this.handleMoneyChange} />
            </td>
            <td></td>
            <td>
                <Button.Group floated='right'>
                    <Button onClick={this.handleCancel} icon='cancel' content='Cancel' />
                    <Button onClick={this.handleSave} icon='save' content='Save' positive />
                </Button.Group>
            </td>
        </tr>;
    }

    handleInputChange = (event: any, data: any) => {
        const { name, value } = data;

        this.setState({ [name]: value });
    }

    handleMoneyChange = (data: any) => {
        const { name, amount, currencyCode } = data;
        let obj: any = {
            [name]: amount,
            [name + 'Currency']: currencyCode
        };

        this.setState(obj);
    }

    handleSave = () => {
        this.props.onSave({
            ...this.state
        });
    }

    handleCancel = () => {
        this.props.onCancel();
    }
}