import * as React from 'react';
import { NavLink } from 'react-router-dom';
import * as classNames from 'classnames';
import { pluralizeMonth } from '../Utils';

interface IMonthNavProps {
    currentYear: number;
    currentMonth: number;
    onMonthClick(year: number, month: number): void;
}

export class MonthNav extends React.Component<IMonthNavProps, {}> {
    public render() {
        const renderItem = (i: number) => this.renderItemFromNow(i);
        const prev = [-3, -2, -1].map(renderItem);
        const next = [1, 2, 3].map(renderItem);

        return <ul>
            {prev}
            {next}
        </ul>;
    }

    renderItemFromNow(diff: number) {
        const { year, month } = this.getMonthFromCurrent(diff);

        return <MonthNavItem key={year + '-' + month} currentYear={year} currentMonth={month} onMonthClick={this.props.onMonthClick} />
    }

    getMonthFromCurrent(diff: number) {
        const before = diff < 0;
        let year = this.props.currentYear;
        let month = this.props.currentMonth + diff;
        if (month < 1) {
            year -= 1;
            month += 12;
        }
        if (month > 12) {
            year += 1;
            month -= 12;
        }

        return { year, month };
    }
}

class MonthNavItem extends React.Component<IMonthNavProps> {
    constructor(props: any) {
        super(props);

        this.onClick = this.onClick.bind(this);
    }

    public render() {
        return <li className='month-nav__item' onClick={this.onClick}>{pluralizeMonth(this.props.currentMonth)} - {this.props.currentYear}</li>;
    }

    onClick() {
        this.props.onMonthClick(this.props.currentYear, this.props.currentMonth);
    }
}