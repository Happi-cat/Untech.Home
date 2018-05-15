import Store, {thunk} from 'repatch';
import {State} from "./types";

export default new Store<State>({
  dailyCalendarFrom: -20,
  dailyCalendarTo: 40,

  isFetching: false,
}).addMiddleware(thunk);