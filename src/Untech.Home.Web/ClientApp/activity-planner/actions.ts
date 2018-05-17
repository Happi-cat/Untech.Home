import {Dispatch} from "repatch";
import {apiService, IActivityOccurrence} from "./api";
import {State, ActivityPlannerReducer as Reducer, ActivityPlannerThunk as Thunk} from "./types";

function changeState(partialState: Partial<State>): Reducer {
  return state => ({
    ...state,
    ...partialState
  });
}

function showSpinner() {
  return changeState({isFetching: true});
}

function hideSpinner() {
  return changeState({isFetching: false});
}

export function fetchCalendar(): Thunk<Promise<void>> {
  return state => async (dispatch: Dispatch<State>) => {
    dispatch(unselectActivityOccurrence());
    dispatch(showSpinner());

    const monthlyCalendar = await apiService.getMonthlyCalendar(-18, 2);
    const dailyCalendar = await apiService.getDailyCalendar(state.dailyCalendarFrom, state.dailyCalendarTo);

    dispatch(changeState({monthlyCalendar, dailyCalendar}));
    dispatch(hideSpinner());
  }
}

export function changeDailyCalendarRange(from: number, to: number): Thunk<Promise<void>> {
  return () => async (dispatch: Dispatch<State>) => {
    dispatch(changeState({
      dailyCalendarFrom: from,
      dailyCalendarTo: to
    }));
    await dispatch(fetchCalendar());
  }
}

export function selectActivityOccurrence(occurrence: IActivityOccurrence): Reducer {
  return changeState({selectedActivityOccurrence: occurrence});
}

export function unselectActivityOccurrence(): Reducer {
  return changeState({selectedActivityOccurrence: undefined});
}

export function addGroup(name: string): Thunk<Promise<void>> {
  return () => async (dispatch: Dispatch<State>) => {
    await apiService.createGroup({name: name});
    await dispatch(fetchCalendar());
  }
}

export function updateGroup(id: number, name: string): Thunk<Promise<void>> {
  return () => async (dispatch: Dispatch<State>) => {
    await apiService.updateGroup({
      key: id,
      name: name
    });
    await dispatch(fetchCalendar());
  }
}

export function deleteGroup(id: number): Thunk<Promise<void>> {
  return () => async (dispatch: Dispatch<State>) => {
    await apiService.deleteGroup(id);
    await dispatch(fetchCalendar());
  }
}

export function addActivity(groupId: number, name: string): Thunk<Promise<void>> {
  return () => async (dispatch: Dispatch<State>) => {
    await apiService.createActivity({
      groupKey: groupId,
      name: name
    });
    await dispatch(fetchCalendar());
  }
}

export function updateActivity(id: number, name: string): Thunk<Promise<void>> {
  return () => async (dispatch: Dispatch<State>) => {
    await apiService.updateActivity({
      key: id,
      name: name
    });
    await dispatch(fetchCalendar());
  }
}

export function deleteActivity(id: number): Thunk<Promise<void>> {
  return () => async (dispatch: Dispatch<State>) => {
    await apiService.deleteActivity(id)
    await dispatch(fetchCalendar());
  }
}

export function toggleActivityOccurrence(activityId: number, year: number, month: number, day: number): Thunk<Promise<void>> {
  return () => async (dispatch: Dispatch<State>) => {
    await apiService.toggleActivityOccurrence({
      activityKey: activityId,
      when: `${year}-${month}-${day}T00:00:00Z`
    });
    await dispatch(fetchCalendar());
  }
}

export function updateActivityOccurrence(occurrence: IActivityOccurrence): Thunk<Promise<void>> {
  return () => async (dispatch: Dispatch<State>) => {
    await apiService.updateActivityOccurrence({
      key: occurrence.key,
      note: occurrence.note,
      highlighted: occurrence.highlighted,
      missed: occurrence.missed
    });

    dispatch(unselectActivityOccurrence());
    await dispatch(fetchCalendar());
  }
}