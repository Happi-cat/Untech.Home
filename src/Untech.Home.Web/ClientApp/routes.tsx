import * as React from 'react';
import {Route} from 'react-router-dom';
import {Layout} from './components/Layout';
import {Home} from './components/Home';
import {AnnualFinancialReport} from './financial-planner/AnnualFinancialReport';
import {MonthlyFinancialReport} from './financial-planner/MonthlyFinancialReport';
import {FinancialJournal} from './financial-planner/FinancialJournal';
import ActivityPlanner from "./activity-planner/Planner";
import {Provider} from "react-redux";
import store from "./activity-planner/store";

export const routes = <Layout>
  <Route exact path='/' component={Home}/>
  <Route exact path='/financial-planner' component={AnnualFinancialReport}/>
  <Route exact path='/financial-planner/:year/:month' component={MonthlyFinancialReport}/>
  <Route exact path='/financial-planner/journal/:year/:month/:taxonId' component={FinancialJournal}/>
  <Route exact path='/activity-planner' component={ActivityPlannerRoute}/>
</Layout>;

function ActivityPlannerRoute() {
  return <Provider store={store as any}>
    < ActivityPlanner/>
  </Provider>
}