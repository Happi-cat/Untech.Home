import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { ITaxonTree, IFinancialJournalEntry, apiService } from './api';
import { JournalMenu } from './financial-journal/JournalMenu';
import { Journal, IFinancialJournalEntryChange } from './financial-journal/Journal';
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
        const { year, month, taxonId } = props;

        Promise.all([
            apiService.getJournal(year, month, taxonId),
            apiService.getTaxon(taxonId, 1)
        ]).then((res) => {
            let [entries, taxon] = res;

            this.setState({
                loading: false,
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
    public componentWillReceiveProps(newProps: IFinancialJournalInnerProps) {
        console.log('inner.newProps', newProps);
    }

    public render() {
        const { year, month, taxon, entries } = this.props;

        return <Grid>

            <Grid.Column width={12}>
                <Journal editable={taxon.isSelectable} entries={entries}
                    onAdd={this.onJournalEntryAdd}
                    onUpdate={this.onJournalEntryUpdate}
                    onDelete={this.onJournalEntryDelete} />
            </Grid.Column>
            <Grid.Column width={4} >
                <JournalMenu year={year} month={month} taxon={taxon} />
            </Grid.Column>
        </Grid>;
    }

    onJournalEntryAdd = (model: IFinancialJournalEntryChange) => {
        const { year, month, taxon } = this.props;

        apiService.createJournalEntry({
            remarks: model.remarks,
            actual: model.actual,
            forecasted: model.forecasted,
            taxonKey: taxon.key,
            year: year,
            month: month
        }).then(data => {
            this.raiseOnUpdated(this.props.entries.concat(data))
        });
    }

    onJournalEntryUpdate = (id: number, model: IFinancialJournalEntryChange) => {
        apiService.updateJournalEntry({
            key: id,
            remarks: model.remarks,
            actual: model.actual,
            forecasted: model.forecasted,
        }).then(data => {
            this.raiseOnUpdated(this.props.entries.map(n => n.key == id ? data : n))
        });
    }

    onJournalEntryDelete = (id: number) => {
        apiService.deleteJournalEntry({
            key: id
        }).then(() => {
            this.raiseOnUpdated(this.props.entries.filter(i => i.key != id))
        });
    }

    raiseOnUpdated(entries: IFinancialJournalEntry[]) {
        console.log('raiseOnUpdated', this);
        const { onUpdated } = this.props;
        onUpdated && onUpdated(entries);
    }
}