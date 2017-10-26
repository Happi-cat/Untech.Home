import * as React from 'react';
import { NavLink } from 'react-router-dom';
import * as classNames from 'classnames';
import { MonthView } from '../components';

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

        return <nav >
            <ul className='pagination' >
                {prev}
                <li className='page-item active'><span className='page-link'><MonthView year={this.props.currentYear} month={this.props.currentMonth} /></span></li>
                {next}
            </ul>
        </nav>;
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
    public render() {
        return <li className='page-item' onClick={this.handleClick}>
            <span className='page-link'>
                <MonthView year={this.props.currentYear} month={this.props.currentMonth} />
            </span>
        </li>;
    }

    handleClick = () => {
        this.props.onMonthClick(this.props.currentYear, this.props.currentMonth);
    }
}