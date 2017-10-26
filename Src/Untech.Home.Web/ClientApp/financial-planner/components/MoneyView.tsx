import * as React from 'react';
import * as classNames from 'classnames';
import { humanizeNumber } from '../../Utils';
import './MoneyView.less';

interface IMoneyViewProps {
    amount: number;
    currencyCode: string;
    className?: string;
};

export function MoneyView(props: IMoneyViewProps) {
    const { amount, currencyCode } = props;
    const cls = classNames('money-view', props.className, {
        'money-view--zero': amount == 0
    });

    return <span className={cls}>
        <span className='money-view__amount'>{humanizeNumber(amount, 2)}</span>
        <span className='money-view__currency'>{currencyCode}</span>
    </span>;
}

export default MoneyView;