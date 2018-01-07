export interface IActivityOccurrence {
  key: number;
  activityKey: number;
  when: Date;
  note?: string;
  highlighted: boolean;
  missed: boolean;
  ongoing: boolean;
}

export interface IActivitiesViewActivity {
  activityKey: number;
  name: string;
}

export interface IActivitiesViewGroup {
  groupKey: number;
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
  activityKey: number;
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
  groupKey: number;
}

export interface ICreateGroup {
  name: string;
}

export interface IToogleActivityOccurrence {
  activityKey: number;
  when: Date;
}

export interface IToogleActivityOccurrences {
  activityKey: number;
  when: Date[];
}

export interface IUpdateActivityOccurrence {
  key: number;
  note?: string;
  highlighted: boolean;
  missed: boolean;
}