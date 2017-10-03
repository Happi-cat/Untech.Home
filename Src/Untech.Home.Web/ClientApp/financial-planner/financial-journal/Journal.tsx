import * as React from 'react';
import { IMoney, IFinancialJournalEntry } from '../api/Models';

interface IFinancialJournalEntryChange {
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

            <tbody>
                {this.props.entries.map(e => <JournalItem key={e.id} model={e} editable={this.props.editable}
                    onUpdate={this.props.onUpdate}
                    onDelete={this.props.onDelete} />)}
            </tbody>

            <tfoot>
                {this.props.editable ? this.renderAddItemFooter() : null}
            </tfoot>
        </table>;
    }

    renderAddItemFooter() {
        return <tr>
            <td><input /></td>
            <td><input /></td>
            <td><input /></td>
            <td></td>
            <td><button >Save</button></td>
        </tr>;
    }
}

interface IJournalItemProps {
    model: IFinancialJournalEntry,
    editable: boolean,
    onUpdate(id: number, args: IFinancialJournalEntryChange): void;
    onDelete(id: number): void;
}

interface IJournalItemState {
    edit: boolean
}

class JournalItem extends React.Component<IJournalItemProps, IJournalItemState> {
    public render() {

        if (this.state.edit) {
            return <tr>

            </tr>;
        }

        return <tr></tr>;
    }
}