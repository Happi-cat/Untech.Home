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
  IUpdateActivity
} from './Models';

export default class ApiService {
  public getDailyCalendar(fromDay: number | string, toDay: number | string) {
    return fetch('api/activity-planner/calendar/daily/' + fromDay + '-' + toDay)
      .then(response => response.json() as Promise<IDailyCalendar>);
  }

  public getMonthlyCalendar(fromMonth: number | string, toMonth: number | string) {
    return fetch('api/activity-planner/calendar/monthly/' + fromMonth + '-' + toMonth)
      .then(response => response.json() as Promise<IMonthlyCalendar>);
  }

  public createGroup(request: ICreateGroup) {
    return fetch('api/activity-planner/group', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(request)
    });
  }

  public updateGroup(request: IUpdateGroup) {
    return fetch('api/activity-planner/group/' + request.key, {
      method: 'PUT',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(request)
    });
  }

  public deleteGroup(request: number) {
    return fetch('api/activity-planner/group/' + encodeURIComponent(request.toString()), { method: 'DELETE' })
      .then(response => response.json() as Promise<boolean>);
  }

  public createActivity(request: ICreateActivity) {
    return fetch('api/activity-planner/activity', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(request)
    });
  }

  public updateActivity(request: IUpdateActivity) {
    return fetch('api/activity-planner/activity/' + request.key, {
      method: 'PUT',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(request)
    });
  }


  public deleteActivity(request: number) {
    return fetch('api/activity-planner/activity/' + encodeURIComponent(request.toString()), { method: 'DELETE' })
      .then(response => response.json() as Promise<boolean>);
  }

  public toggleActivityOccurrence(request: IToogleActivityOccurrence) {
    return fetch('api/activity-planner/activity/' + encodeURIComponent(request.activityKey.toString()) + '/toggle-occurrence', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(request.when)
    });
  }

  public toggleActivityOccurrences(request: IToogleActivityOccurrences) {
    return fetch('api/activity-planner/activity/' + encodeURIComponent(request.activityKey.toString()) + '/toggle-occurrences', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(request.when)
    });
  }

  public updateActivityOccurrence(request: IUpdateActivityOccurrence) {
    return fetch('api/activity-planner/occurrence/' + encodeURIComponent(request.key.toString()), {
      method: 'PUT',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(request)
    });
  }
}