import {Reducer, Thunk} from 'repatch';
import {IActivityOccurrence, IDailyCalendar, IMonthlyCalendar, IToogleActivityOccurrences} from "./api";

export interface State {
  monthlyCalendar?: IMonthlyCalendar;

  dailyCalendar?: IDailyCalendar;
  dailyCalendarFrom: number;
  dailyCalendarTo: number;

  isFetching: boolean;
  error?: string | null;

  selectedActivityOccurrence?: IActivityOccurrence;
}

export type ActivityPlannerReducer = Reducer<State>
export type ActivityPlannerThunk<T> = Thunk<State, {}, T>