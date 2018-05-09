export type GroupKey = number;
export type ActivityKey = number;
export type OccurrenceKey = number;

export interface IActivityOccurrence {
  key: OccurrenceKey;
  activityKey: number;
  when: Date;
  note?: string;
  highlighted: boolean;
  missed: boolean;
  ongoing: boolean;
}

export interface IActivitiesViewActivity {
  activityKey: ActivityKey;
  name: string;
}

export interface IActivitiesViewGroup {
  groupKey: GroupKey;
  name: string;
  activities: IActivitiesViewActivity[]
}

export interface IActivitiesView {
  groups: IActivitiesViewGroup[]
}

export interface IDailyCalendarDay {
  year: number;
  month: number;
  day: number;
  dayOfWeek: number;
  isThisDay: boolean;
  activities: IActivityOccurrence[]
}

export interface IDailyCalendarMonth {
  year: number;
  month: number;
  days: IDailyCalendarDay[];
}


export interface IDailyCalendar {
  view: IActivitiesView;
  months: IDailyCalendarMonth[];
}

export interface IMonthlyCalendarMonthActivity {
  activityKey: ActivityKey;
  count: number;
}

export interface IMonthlyCalendarMonth {
  year: number;
  month: number;
  isThisMonth: boolean;
  activities: IMonthlyCalendarMonthActivity[];
}

export interface IMonthlyCalendar {
  view: IActivitiesView;
  months: IMonthlyCalendarMonth[];
}

export interface ICreateActivity {
  name: string;
  groupKey: GroupKey;
}

export interface ICreateGroup {
  name: string;
}

export interface IUpdateActivity {
  key: GroupKey;
  name: string;
}

export interface IUpdateGroup {
  key: GroupKey;
  name: string;
}

export interface IToogleActivityOccurrence {
  activityKey: ActivityKey;
  when: Date | string;
}

export interface IToogleActivityOccurrences {
  activityKey: ActivityKey;
  when: Date[] | string[];
}

export interface IUpdateActivityOccurrence {
  key: OccurrenceKey;
  note?: string;
  highlighted: boolean;
  missed: boolean;
}