import 'isomorphic-fetch';
import {
  IDailyCalendar,
  IMonthlyCalendar,
  ICreateActivity,
  ICreateGroup,
  IUpdateActivityOccurrence,
  IToogleActivityOccurrence,
  IToogleActivityOccurrences,
  IUpdateGroup,
  IUpdateActivity, GroupKey, ActivityKey
} from './Models';

export default class ApiService {
  public getDailyCalendar(fromDay: number | string, toDay: number | string) {
    return this.fetchJson('calendar/daily/range(' + fromDay + ';' + toDay + ')')
      .then(response => response.json() as Promise<IDailyCalendar>);
  }

  public getMonthlyCalendar(fromMonth: number | string, toMonth: number | string) {
    return this.fetchJson('calendar/monthly/range(' + fromMonth + ';' + toMonth + ')')
      .then(response => response.json() as Promise<IMonthlyCalendar>);
  }

  public createGroup(request: ICreateGroup) {
    return this.fetchJson('group', 'POST', request);
  }

  public updateGroup(request: IUpdateGroup) {
    return this.fetchJson('group/' + request.key, 'PUT', request);
  }

  public deleteGroup(request: GroupKey) {
    return this.fetchJson('group/' + encodeURIComponent(request.toString()), 'DELETE')
      .then(response => response.json() as Promise<boolean>);
  }

  public createActivity(request: ICreateActivity) {
    return this.fetchJson('activity', 'POST', request);
  }

  public updateActivity(request: IUpdateActivity) {
    return this.fetchJson('activity/' + request.key, 'PUT', request);
  }


  public deleteActivity(request: ActivityKey) {
    return this.fetchJson('activity/' + encodeURIComponent(request.toString()), 'DELETE')
      .then(response => response.json() as Promise<boolean>);
  }

  public toggleActivityOccurrence(request: IToogleActivityOccurrence) {
    return this.fetchJson('activity/' + encodeURIComponent(request.activityKey.toString()) + '/toggle-occurrence', 'POST', request.when);
  }

  public updateActivityOccurrence(request: IUpdateActivityOccurrence) {
    return this.fetchJson('occurrence/' + encodeURIComponent(request.key.toString()), 'PUT', request);
  }

  fetchJson(url: string, method?: string, body?: any) {
    return fetch('api/activity-planner/' + url, {
      method: method || 'GET',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: body ? JSON.stringify(body) : undefined
    });
  }
}