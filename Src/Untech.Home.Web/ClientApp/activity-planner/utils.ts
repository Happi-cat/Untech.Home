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
