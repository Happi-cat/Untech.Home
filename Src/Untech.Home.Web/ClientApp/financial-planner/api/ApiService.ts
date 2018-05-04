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
    return this.fetchJson('report')
      .then(response => response.json() as Promise<IAnnualFinancialReport>);
  }

  public getMonthlyReport(year: number | string, month: number | string) {
    let url = 'report/' + encodeURIComponent(year.toString()) + '/' + encodeURIComponent(month.toString());
    return this.fetchJson(url)
      .then(response => response.json() as Promise<IMonthlyFinancialReport>);
  }

  public getTaxon(taxonKey?: number | string, deep?: number) {
    let url = 'taxon';
    if (taxonKey != undefined) {
      url += '/' + encodeURIComponent(taxonKey.toString());

      if (deep != undefined) {
        url += '?deep=' + encodeURIComponent(deep.toString());
      }
    }

    return this.fetchJson(url)
      .then(response => response.json() as Promise<ITaxonTree>);
  }

  public getJournal(year: number | string, month: number | string, taxonKey?: number | string, deep?: number) {
    let url = 'journal/' + encodeURIComponent(year.toString()) + '/' + encodeURIComponent(month.toString());
    if (taxonKey != undefined) {
      url += '?taxonId=' + encodeURIComponent(taxonKey.toString());

      if (deep != undefined) {
        url += '&deep=' + encodeURIComponent(deep.toString());
      }
    }

    return this.fetchJson(url)
      .then(response => response.json() as Promise<IFinancialJournalEntry[]>);
  }

  public createJournalEntry(request: ICreateFinancialJournalEntryCommand) {
    return this.fetchJson('journal', 'POST', request)
      .then(response => response.json() as Promise<IFinancialJournalEntry>);
  }

  public updateJournalEntry(request: IUpdateFinancialJournalEntryCommand) {
    return this.fetchJson('journal/' + encodeURIComponent(request.key.toString()), 'PUT', request)
      .then(response => response.json() as Promise<IFinancialJournalEntry>);;
  }

  public deleteJournalEntry(request: IDeleteFinancialJournalEntryCommand) {
    return this.fetchJson('journal/' + encodeURIComponent(request.key.toString()), 'DELETE')
      .then(response => response.json() as Promise<boolean>);
  }

  fetchJson(url: string, method?: string, body?: any) {
    return fetch('api/financial-planner/' + url, {
      method: method || 'GET',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: body ? JSON.stringify(body) : undefined
    });
  }
}