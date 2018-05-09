const moDict: { [mo: number]: string } = {
  1: 'Jan',
  2: 'Feb',
  3: 'Mar',
  4: 'Apr',
  5: 'May',
  6: 'Jun',
  7: 'Jul',
  8: 'Aug',
  9: 'Sep',
  10: 'Oct',
  11: 'Nov',
  12: 'Dec'
};

const dayOfWeekDict: { [d: number]: string } = {
  1: 'Mo',
  2: 'Tu',
  3: 'We',
  4: 'Th',
  5: 'Fr',
  6: 'Sa',
  7: 'Su'
};

export function pluralizeMonth(month: number) {
  return moDict[month];
}

export function pluralizeDayOfWeek(dayOfWeek: number) {
  return dayOfWeekDict[dayOfWeek];
}

export function humanizeNumber(value: number | string, fractionDigits?: number) {
  const int = Number.parseFloat((value || 0).toString());

  return int
    .toFixed(fractionDigits)
    .replace(/\B(?=(\d{3})+(?!\d))/g, " ");
}

export function humanizeDate(value: Date | string) {
  const date = new Date(value.toString());

  return date.toDateString();
}