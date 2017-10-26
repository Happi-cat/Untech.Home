import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { ITaxonTree, IFinancialJournalEntry, apiService } from './api';
import { MonthNav } from './financial-journal/MonthNav';
import { TaxonNav } from './financial-journal/TaxonNav';
import { Journal, IFinancialJournalEntryChange } from './financial-journal/Journal';
import { pluralizeMonth } from './../utils';

interface IFinancialJournalProps {
    taxonId: string,
    year: string,
    month: string
}

interface IFinancialJournalState {
    currentTaxon?: ITaxonTree;
    journalEntries?: IFinancialJournalEntry[]
}

export class FinancialJournal extends React.Component<RouteComponentProps<IFinancialJournalProps>, IFinancialJournalState> {
    taxonId: number;
    year: number;
    month: number;

    constructor(props: any) {
        super(props);

        this.state = {};

        this.changeMonth = this.changeMonth.bind(this);
        this.changeTaxon = this.changeTaxon.bind(this);
        this.onJournalEntryAdd = this.onJournalEntryAdd.bind(this);
        this.onJournalEntryUpdate = this.onJournalEntryUpdate.bind(this);
        this.onJournalEntryDelete = this.onJournalEntryDelete.bind(this);
    }

    public componentWillMount() {
        this.taxonId = Number.parseInt(this.props.match.params.taxonId);
        this.year = Number.parseInt(this.props.match.params.year);
        this.month = Number.parseInt(this.props.match.params.month);

        this.load();
    }

    public render() {

        if (!this.state.currentTaxon || !this.state.journalEntries) {
            return <div>Loading...</div>;
        }

        return <div >
            {this.renderInfo()}
            <div><h4>Months</h4><MonthNav currentMonth={this.month} currentYear={this.year} onMonthClick={this.changeMonth} /></div>
            <div><h4>Taxons</h4><TaxonNav model={this.state.currentTaxon} onClick={this.changeTaxon} /></div>
            <Journal editable={this.state.currentTaxon.isSelectable} entries={this.state.journalEntries}
                onAdd={this.onJournalEntryAdd}
                onUpdate={this.onJournalEntryUpdate}
                onDelete={this.onJournalEntryDelete} />
        </div>;
    }

    renderInfo() {
        if (!this.state.currentTaxon) {
            return <div>Oops...</div>
        }

        const taxon = this.state.currentTaxon;

        return <div className='h4'>
            <span className='journal-info__taxon-name'> {taxon.name}</span>
            <span className='journal-info__year'> {this.year}</span>
            <span className='journal-info__month'> {pluralizeMonth(this.month)}</span>
        </div>
    }

    changeMonth(year: number, month: number) {
        const { taxonId } = this.props.match.params;
        this.props.history.push('/financial-planner/journal/' + year + '/' + month + '/' + taxonId);
    }

    changeTaxon(taxonId: number) {
        const { year, month } = this.props.match.params;
        this.props.history.push('/financial-planner/journal/' + year + '/' + month + '/' + taxonId);
    }

    onJournalEntryAdd(model: IFinancialJournalEntryChange) {
        const { year, month, taxonId } = this.props.match.params;

        apiService.createJournalEntry({
            remarks: model.remarks,
            actual: model.actual,
            forecasted: model.forecasted,
            taxonId: Number.parseInt(taxonId),
            year: Number.parseInt(year),
            month: Number.parseInt(month)
        }).then(() => this.load());
    }

    onJournalEntryUpdate(id: number, model: IFinancialJournalEntryChange) {
        apiService.updateJournalEntry({
            id: id,
            remarks: model.remarks,
            actual: model.actual,
            forecasted: model.forecasted,
        }).then(() => this.load());
    }

    onJournalEntryDelete(id: number) {
        apiService.deleteJournalEntry({ id: id })
            .then(() => this.load());
    }

    load() {
        const { year, month, taxonId } = this.props.match.params;

        Promise.all([
            apiService.getJournal(year, month, taxonId),
            apiService.getTaxon(this.taxonId, 1)
        ]).then((res) => {
            let [entries, taxon] = res;

            this.setState({
                currentTaxon: taxon,
                journalEntries: entries
            });
        });
    }
}
