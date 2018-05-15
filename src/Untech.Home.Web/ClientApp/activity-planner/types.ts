import { Reducer, Thunk  } from 'repatch';
import {IActivityOccurrence, IDailyCalendar, IMonthlyCalendar} from "./api";

export interface ICa {

}

export interface State {
  monthlyCalendar?: IMonthlyCalendar;

  dailyCalendar?: IDailyCalendar;
  dailyCalendarFrom: number;
  dailyCalendarTo: number;

  isFetching: boolean;
  error?: string | null;

  selectedActivityOccurrnece?: IActivityOccurrence;
}

export type ActivityPlannerReducer = Reducer<State>
export type ActivityPlannerThunk<T> = Thunk<State, {}, T>