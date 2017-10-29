import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { ITaxonTree, IFinancialJournalEntry, apiService } from './api';
import { JournalMenu } from './financial-journal/JournalMenu';
import { Journal, IFinancialJournalEntryChange } from './financial-journal/Journal';
import { pluralizeMonth } from './../utils';
import { Grid } from 'semantic-ui-react';

interface IFinancialJournalProps {
    taxonId: string,
    year: string,
    month: string
}

interface IFinancialJournalState {
    loading: boolean;
    taxon?: ITaxonTree;
    entries?: IFinancialJournalEntry[]
}

export class FinancialJournal extends React.Component<RouteComponentProps<IFinancialJournalProps>, IFinancialJournalState> {
    mounted = true;

    constructor(props: any) {
        super(props);

        this.state = { loading: true };
    }

    public componentWillMount() {
        this.load(this.props.match.params);
    }

    public componentWillReceiveProps(props: RouteComponentProps<IFinancialJournalProps>) {
        this.load(props.match.params);
    }

    public render() {
        const { taxon, entries } = this.state;

        if (!taxon || !entries) {
            return <div>Loading...</div>;
        }

        const { year, month } = this.props.match.params;

        return <FinancialJournalInner
            year={Number.parseInt(year)}
            month={Number.parseInt(month)}
            taxon={taxon}
            entries={entries}
            onUpdated={this.handleUpdate} />;
    }

    handleUpdate = (entries: IFinancialJournalEntry[]) => {
        this.setState({ entries: entries });
    }

    load(props: IFinancialJournalProps) {
        if (!this.mounted) {
            return;
        }

        const { year, month, taxonId } = props;

        Promise.all([
            apiService.getJournal(year, month, taxonId),
            apiService.getTaxon(taxonId, 1)
        ]).then((res) => {
            let [entries, taxon] = res;

            this.setState({
                taxon: taxon,
                entries: entries || []
            });
        });
    }
}


interface IFinancialJournalInnerProps {
    year: number;
    month: number;
    taxon: ITaxonTree;
    entries: IFinancialJournalEntry[],
    onUpdated?(entries: IFinancialJournalEntry[]): void;
}

class FinancialJournalInner extends React.Component<IFinancialJournalInnerProps> {
    public render() {
        const { year, month, taxon, entries } = this.props;

        return <Grid>
            <Grid.Column width={2} >
                <JournalMenu year={year} month={month} taxon={taxon} />
            </Grid.Column>
            <Grid.Column width={14}>
                <Journal editable={taxon.isSelectable} entries={entries}
                    onAdd={this.onJournalEntryAdd}
                    onUpdate={this.onJournalEntryUpdate}
                    onDelete={this.onJournalEntryDelete} />
            </Grid.Column>
        </Grid>;
    }

    onJournalEntryAdd = (model: IFinancialJournalEntryChange) => {
        const { year, month, taxon } = this.props;

        apiService.createJournalEntry({
            remarks: model.remarks,
            actual: model.actual,
            forecasted: model.forecasted,
            taxonId: taxon.id,
            year: year,
            month: month
        }).then(data => {
            this.raiseOnUpdated(this.props.entries.concat(data))
        });
    }

    onJournalEntryUpdate = (id: number, model: IFinancialJournalEntryChange) => {
        apiService.updateJournalEntry({
            id: id,
            remarks: model.remarks,
            actual: model.actual,
            forecasted: model.forecasted,
        }).then(data => {
            this.raiseOnUpdated(this.props.entries.map(n => n.id == id ? data : n))
        });
    }

    onJournalEntryDelete = (id: number) => {
        apiService.deleteJournalEntry({
            id: id
        }).then(() => {
            this.raiseOnUpdated(this.props.entries.filter(i => i.id != id))
        });
    }

    raiseOnUpdated(entries: IFinancialJournalEntry[]) {
        const { onUpdated } = this.props;
        onUpdated && onUpdated(entries);
    }
}