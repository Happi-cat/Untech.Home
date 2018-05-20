import {Store, thunk} from "repatch";
import {State} from "./types";

export default new Store<State>({
  isFetching: false,
  expandedEntries: [],

  monthlyReport: {
    isSelected: false,
    selectedYear: 0,
    selectedMonth: 0,
    isFetching: false
  },

  journal: {
    isSelected: false,
    selectedTaxon: 0,
    selectedYear: 0,
    selectedMonth: 0,
    isFetching: false
  }
}).addMiddleware(thunk)