import * as React from 'react';
import { Input, Dropdown } from 'semantic-ui-react';

interface IMoneyInputProps {
    amount?: number;
    currencyCode?: string;
    onChange(amount: number, currencyCode: string): void;
}

interface IMoneyInputState {
    amount: number;
    currencyCode: string;
}

const currencies = [
    { key: 'BYN', text: 'BYN', value: 'BYN' },
];

export class MoneyInput extends React.Component<IMoneyInputProps, IMoneyInputState> {
    public render() {
        return <Input
            label={<Dropdown defaultValue={this.props.currencyCode} options={currencies} />}
            labelPosition='right'
            placeholder='Amount'
            defaultValue={this.props.amount}
            onChange={this.handleAmountChange}
        />;
    }

    handleAmountChange = (e: any, data: any) => {
        console.log(data);
    }
}

export default MoneyInput;