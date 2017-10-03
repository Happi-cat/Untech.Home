import 'isomorphic-fetch';
import {
  ITaxonTree,
  IAnnualFinancialReport,
  IFinancialJournalEntry,
  ICreateFinancialJournalEntryCommand,
  IUpdateFinancialJournalEntryCommand,
  IDeleteFinancialJournalEntryCommand
} from './Models';

export class FinancialPlannerApiService {
  public getReport() {
    return fetch('api/financial-planner/report')
      .then(response => response.json() as Promise<IAnnualFinancialReport>);
  }

  public getTaxon(taxonId?: number, deep?: number) {
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

  public getJournal(year: number, month: number, taxonId?: number, deep?: number) {
    return fetch('api/financial-planner/journal/' + encodeURIComponent(year.toString()) + '/' + encodeURIComponent(month.toString()))
      .then(response => response.json() as Promise<IFinancialJournalEntry[]>);
  }

  public createJournalEntry(request: ICreateFinancialJournalEntryCommand) {
    return fetch('api/financial-planner/journal', { method: 'POST', body: request });
  }

  public updateJournalEntry(request: IUpdateFinancialJournalEntryCommand) {
    return fetch('api/financial-planner/journal/' + encodeURIComponent(request.id.toString()), { method: 'PUT', body: request });
  }

  public deleteJournalEntry(request: IDeleteFinancialJournalEntryCommand) {
    return fetch('api/financial-planner/journal/' + encodeURIComponent(request.id.toString()), { method: 'DELETE' });
  }
}