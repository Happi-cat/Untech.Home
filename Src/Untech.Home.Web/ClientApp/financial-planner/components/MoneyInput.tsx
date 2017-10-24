import * as React from 'react';

interface IMoneyInputProps {
    amount?: number;
    currencyCode?: string;
    currencyCodes: string[];
    onChange(amount: number, currencyCode: string): void;
}

interface IMoneyInputState {
    amount: number;
    currencyCode: string;
}

export class MoneyInput extends React.Component<IMoneyInputProps, IMoneyInputState> {
    public render() {
        return <input type='text' />;
    }
}

export default MoneyInput;