import { IActivitiesView, IActivitiesViewActivity } from './api';

export function activitiesViewToDict(request: IActivitiesView) {
  let dict : { [activityKey: number]: IActivitiesViewActivity } = {};

  request.groups.forEach(group => {
    group.activities.forEach(activity => {
      dict[activity.activityKey] = activity;
    });
  });

  return dict;
}

export function activitiesViewToList(request: IActivitiesView) {
  return request.groups
    .map(group => group.activities)
    .reduce((old, current) => [...old, ...current], [])
}

