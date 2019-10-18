import {
  State,
  FinancialReportReducer as Reducer,
  FinancialReportThunk as Thunk,
  MonthlyReportState,
  JournalState, IFinancialJournalEntryChange
} from "./types";
import {Dispatch} from "repatch";
import {apiService, TaxonKey} from "./api";

function changeState(paritalState: Partial<State>): Reducer {
  return state => ({ ...state, ...paritalState });
}

function changeMonthlyReportState(partialState: Partial<MonthlyReportState>) : Reducer {
  return state => ({
    ...state,
    monthlyReport: { ...state.monthlyReport, ...partialState }
  });
}

function changeJournalState(partialState: Partial<JournalState>) : Reducer {
  return state => ({
    ...state,
    journal: { ...state.journal, ...partialState }
  });
}

function invalidateData() : Reducer {
  return state => ({
    ...state,
    annualReport: undefined,
    journal: {
      ...state.journal,
      journal: undefined,
    },
    monthlyReport: {
      ...state.monthlyReport,
      report: undefined
    }
  });
}

function showSpinner() {
  return changeState({ isFetching: true });
}

function hideSpinner() {
  return changeState({isFetching: false });
}

export function fetchReport() : Thunk<Promise<void>> {
  return state => async (dispatch: Dispatch<State>) => {
    if (state.annualReport || state.isFetching) {
      return
    }

    dispatch(showSpinner());

    var data = await apiService.getReport();

    dispatch(changeState({ annualReport: data }));
    dispatch(hideSpinner());
  }
}

export function expandCollapseAnnualReportEntry(taxonKey: TaxonKey) : Reducer {
  return (state : State) => {
    const wasExpanded = state.expandedEntries.indexOf(taxonKey) > -1;
    const expandedEntries = wasExpanded
      ? state.expandedEntries.filter(e => e != taxonKey)
      : state.expandedEntries.concat(taxonKey)

    return {
      ...state,
      expandedEntries
    };
  }
}

export function fetchMonthlyReport(year: number, month: number) : Thunk<Promise<void>> {
  return state => async (dispatch : Dispatch<State>) => {
    const reportState = state.monthlyReport;
    const canSkip = reportState.isSelected
      && reportState.selectedYear == year
      && reportState.selectedMonth == month
      && (reportState.report || reportState.isFetching);

    if (canSkip) {
      return
    }

    dispatch(changeMonthlyReportState({
      isSelected: true,
      selectedYear: year,
      selectedMonth: month,
      isFetching: true
    }));

    const data = await apiService.getMonthlyReport(year, month);

    dispatch(changeMonthlyReportState({
      isFetching: false,
      report: data
    }));
  }
}

export function fetchJournal(taxonKey: TaxonKey, year: number, month: number) : Thunk<Promise<void>> {
  return state => async (dispatch : Dispatch<State>) => {
    const journalState = state.journal;
    const canSkip = journalState.isSelected
      && journalState.selectedTaxon == taxonKey
      && journalState.selectedYear == year
      && journalState.selectedMonth == month
      && (journalState.journal || journalState.isFetching);

    if (canSkip) {
      return;
    }

    dispatch(changeJournalState({
      isSelected: true,
      isFetching: true,
      selectedTaxon: taxonKey,
      selectedYear: year,
      selectedMonth: month
    }));

    const data = await apiService.getJournal(year, month, taxonKey);
    const taxon = await apiService.getTaxon(taxonKey, 1);

    dispatch(changeJournalState({
      isFetching: false,
      journal: data,
      taxon: taxon
    }));
  }
}

export function addJournalEntry(args: IFinancialJournalEntryChange): Thunk<Promise<void>> {
  return (state : State) => async (dispatch: Dispatch<State>) => {
    const journalState = state.journal;

    await apiService.createJournalEntry({
      taxonKey: journalState.selectedTaxon,
      year: journalState.selectedYear,
      month: journalState.selectedMonth,
      actual: args.actual,
      forecasted: args.forecasted,
      remarks: args.remarks
    });

    dispatch(invalidateData());
  }
}

export function  updateJournalEntry(key: number, args: IFinancialJournalEntryChange): Thunk<Promise<void>> {
  return (state : State) => async (dispatch: Dispatch<State>) => {
    await apiService.updateJournalEntry({
      key: key,
      remarks: args.remarks,
      forecasted: args.forecasted,
      actual: args.actual
    });

    dispatch(invalidateData());
  }
};

export function deleteJournalEntry(key: number): Thunk<Promise<void>> {
  return (state : State) => async (dispatch: Dispatch<State>) => {
    await apiService.deleteJournalEntry({
      key: key
    });

    dispatch(invalidateData());
  }
};
