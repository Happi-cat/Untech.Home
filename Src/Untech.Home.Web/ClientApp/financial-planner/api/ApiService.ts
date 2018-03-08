import 'isomorphic-fetch';
import {
  ITaxonTree,
  IAnnualFinancialReport,
  IMonthlyFinancialReport,
  IFinancialJournalEntry,
  ICreateFinancialJournalEntryCommand,
  IUpdateFinancialJournalEntryCommand,
  IDeleteFinancialJournalEntryCommand
} from './Models';

export class ApiService {
  public getReport() {
    return fetch('api/financial-planner/report')
      .then(response => response.json() as Promise<IAnnualFinancialReport>);
  }

  public getMonthlyReport(year: number | string, month: number | string) {
    let url = 'api/financial-planner/report/' + encodeURIComponent(year.toString()) + '/' + encodeURIComponent(month.toString());
    return fetch(url)
      .then(response => response.json() as Promise<IMonthlyFinancialReport>);
  }

  public getTaxon(taxonKey?: number | string, deep?: number) {
    let url = 'api/financial-planner/taxon';
    if (taxonKey != undefined) {
      url += '/' + encodeURIComponent(taxonKey.toString());

      if (deep != undefined) {
        url += '?deep=' + encodeURIComponent(deep.toString());
      }
    }

    return fetch(url)
      .then(response => response.json() as Promise<ITaxonTree>);
  }

  public getJournal(year: number | string, month: number | string, taxonKey?: number | string, deep?: number) {
    let url = 'api/financial-planner/journal/' + encodeURIComponent(year.toString()) + '/' + encodeURIComponent(month.toString());
    if (taxonKey != undefined) {
      url += '?taxonId=' + encodeURIComponent(taxonKey.toString());

      if (deep != undefined) {
        url += '&deep=' + encodeURIComponent(deep.toString());
      }
    }

    return fetch(url)
      .then(response => response.json() as Promise<IFinancialJournalEntry[]>);
  }

  public createJournalEntry(request: ICreateFinancialJournalEntryCommand) {
    return fetch('api/financial-planner/journal', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(request)
    }).then(response => response.json() as Promise<IFinancialJournalEntry>);
  }

  public updateJournalEntry(request: IUpdateFinancialJournalEntryCommand) {
    return fetch('api/financial-planner/journal/' + encodeURIComponent(request.key.toString()), {
      method: 'PUT',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(request)
    }).then(response => response.json() as Promise<IFinancialJournalEntry>);;
  }

  public deleteJournalEntry(request: IDeleteFinancialJournalEntryCommand) {
    return fetch('api/financial-planner/journal/' + encodeURIComponent(request.key.toString()), { method: 'DELETE' })
      .then(response => response.json() as Promise<boolean>);
  }
}