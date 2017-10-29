import * as React from 'react';
import { Input, Dropdown } from 'semantic-ui-react';

interface IMoneyInputProps {
    [key: string]: any;
    fluid?: boolean;
    initialAmount?: number;
    initialCurrencyCode?: string;
    onChange(data: IMoneyInputProps): void;
}

interface IMoneyInputOnChange extends IMoneyInputProps {
    amount: number;
    currencyCode: string;
}

const currencies = [
    { key: 'BYN', text: 'BYN', value: 'BYN' },
];

export class MoneyInput extends React.Component<IMoneyInputProps> {
    public render() {
        const currencySelector = <Dropdown
            defaultValue={this.props.initialCurrencyCode}
            options={currencies}
            onChange={this.handleCurrencyChange} />;

        return <Input
            fluid={this.props.fluid}
            label={currencySelector}
            labelPosition='right'
            placeholder='Amount'
            type='number'
            defaultValue={this.props.initialAmount}
            onChange={this.handleAmountChange}
        />;
    }

    handleAmountChange = (e: any, data: any) => {
        if (data.value != undefined) {
            this.raiseOnChange({ amount: data.value });
        }
    }

    handleCurrencyChange = (e: any, data: any) => {
        if (data.value != undefined) {
            this.raiseOnChange({ currencyCode: data.value });
        }
    }

    raiseOnChange(newData: any) {
        const data = {
            ...this.props,
            amount: this.props.initialAmount || newData.amount,
            currencyCode: this.props.initialCurrencyCode || newData.currencyCode,
        }

        this.props.onChange(data);
    }
}

export default MoneyInput;