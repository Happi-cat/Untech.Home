import * as React from 'react';
import * as classNames from 'classnames';

interface IMoneyProps {
    amount: number;
    currencyCode: string;
    className?: string;
};

export function Money(props: IMoneyProps) {
    const amount = Number.parseFloat((props.amount || 0).toString());
    const text = amount.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, " ");
    const cls = classNames('money', props.className);

    return <span className={cls}>{text} {props.currencyCode}</span>;
}

export default Money;