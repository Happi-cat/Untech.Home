﻿import {Dispatch, GetState} from "repatch";
import {apiService, IActivityOccurrence} from "./api";
import {State, ActivityPlannerReducer as Reducer, ActivityPlannerThunk as Thunk} from "./types";

function changeState(partialState : Partial<State>) : Reducer {
  return state => ({
    ...state,
    ...partialState
  });
}

function fetchCalendar(): Thunk<Promise<void>> {
  return state => async (dispatch: Dispatch<State>) => {
    dispatch(unselectActivityOccurrence());
    dispatch(changeState({ isFetching: true }));

    var montlyCalendar = await apiService.getMonthlyCalendar(-18, 2);
    var dailyCalendar = await apiService.getDailyCalendar(state.dailyCalendarFrom, state.dailyCalendarTo);

    dispatch(state => ({
      ...state,
      montlyCalendar,
      dailyCalendar,
      isFetching: false
    }));
  }
}

export function changeDailyCalendarRange(from: number, to: number): Thunk<Promise<void>> {
  return () => async (dispatch: Dispatch<State>) => {
    dispatch(state => ({
      ...state,
      dailyCalendarFrom: from,
      dailyCalendarTo: to
    }));
    await dispatch(fetchCalendar());
  }
}

export function selectActivityOccurrence(occurrence: IActivityOccurrence): Reducer {
  return changeState({ selectedActivityOccurrnece: occurrence });
}

export function unselectActivityOccurrence(): Reducer {
  return changeState({ selectedActivityOccurrnece: undefined });
}

export function addGroup(name: string) : Thunk<Promise<void>> {
  return () => async (dispatch: Dispatch<State>) => {
    await apiService.createGroup({name: name});
    await dispatch(fetchCalendar());
  }
}

export function updateGroup(id: number,  name: string) : Thunk<Promise<void>> {
  return () => async (dispatch: Dispatch<State>) => {
    await apiService.updateGroup({
      key: id,
      name: name
    });
    await dispatch(fetchCalendar());
  }
}

export function deleteGroup(id: number) : Thunk<Promise<void>> {
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

export function updateActivity(id: number, name: string) : Thunk<Promise<void>> {
  return () => async (dispatch: Dispatch<State>) => {
    await apiService.updateActivity({
      key: id,
      name: name
    });
    await dispatch(fetchCalendar());
  }
}

export function deleteActivity(id: number) : Thunk<Promise<void>> {
    return () => async (dispatch: Dispatch<State>) => {
      await apiService.deleteActivity(id)
      await dispatch(fetchCalendar());
    }
}

export function toggleActivityOccurrence(activityId: number, year: number, month: number, day: number) : Thunk<Promise<void>> {
  return () => async (dispatch: Dispatch<State>) => {
    await apiService.toggleActivityOccurrence({
      activityKey: activityId,
      when: `${year}-${month}-${day}T00:00:00Z`
    });
    await dispatch(fetchCalendar());
  }
}

export function updateActivityOccurrence(occurrence: IActivityOccurrence) : Thunk<Promise<void>> {
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