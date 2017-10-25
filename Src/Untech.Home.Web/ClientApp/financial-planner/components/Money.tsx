import * as React from 'react';
import * as classNames from 'classnames';

interface IMoneyProps {
    amount: number;
    currencyCode: string;
    className?: string;
    showZero?: boolean;
};

export function Money(props: IMoneyProps) {
    const cls = classNames('money', props.className);
    const amount = Number.parseFloat((props.amount || 0).toString());

    if (amount != 0 || props.showZero) {
        const text = amount.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, " ");

        return <span className={cls}>{text} {props.currencyCode}</span>;
    }

    return <span className={cls} />;
}

export default Money;