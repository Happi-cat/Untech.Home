import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { AnnualFinancialReport } from './financial-planner/AnnualFinancialReport';
import { FinancialJournal } from './financial-planner/FinancialJournal';

export const routes = <Layout>
    <Route exact path='/' component={Home} />
    <Route exact path='/financial-planner' component={AnnualFinancialReport} />
    <Route path='/financial-planner/journal/:year/:month/:taxonId' component={FinancialJournal} />
</Layout>;
