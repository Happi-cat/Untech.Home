import 'isomorphic-fetch';
import {
  ITaxonTree,
  IAnnualFinancialReport,
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

  public getTaxon(taxonId?: number | string, deep?: number) {
    let url = 'api/financial-planner/taxon';
    if (taxonId != undefined) {
      url += '/' + encodeURIComponent(taxonId.toString());

      if (deep != undefined) {
        url += '?deep=' + encodeURIComponent(deep.toString());
      }
    }

    return fetch(url)
      .then(response => response.json() as Promise<ITaxonTree>);
  }

  public getJournal(year: number | string, month: number | string, taxonId?: number | string, deep?: number) {
    let url = 'api/financial-planner/journal/' + encodeURIComponent(year.toString()) + '/' + encodeURIComponent(month.toString());
    if (taxonId != undefined) {
      url += '?taxonId=' + encodeURIComponent(taxonId.toString());

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
    return fetch('api/financial-planner/journal/' + encodeURIComponent(request.id.toString()), {
      method: 'PUT',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(request)
    }).then(response => response.json() as Promise<IFinancialJournalEntry>);;
  }

  public deleteJournalEntry(request: IDeleteFinancialJournalEntryCommand) {
    return fetch('api/financial-planner/journal/' + encodeURIComponent(request.id.toString()), { method: 'DELETE' })
      .then(response => response.json() as Promise<boolean>);
  }
}