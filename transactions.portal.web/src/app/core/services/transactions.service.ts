import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateDisputeRequest, DisputeDetail } from '../models/dispute.model';
import { PagedResult } from '../models/paged-result.model';
import { TransactionDetail, TransactionListItem } from '../models/transaction.model';

@Injectable({
  providedIn: 'root'
})
export class TransactionsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/transactions';

  getTransactions(
    search?: string,
    status?: string,
    fromDate?: string,
    toDate?: string,
    page: number = 1,
    pageSize: number = 10
  ): Observable<PagedResult<TransactionListItem>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if (search && search.trim()) {
      params = params.set('search', search.trim());
    }

    if (status && status.trim() && status !== 'All') {
      params = params.set('status', status.trim());
    }

    if (fromDate) {
      params = params.set('fromDate', fromDate);
    }

    if (toDate) {
      params = params.set('toDate', toDate);
    }

    return this.http.get<PagedResult<TransactionListItem>>(this.baseUrl, { params });
  }

  getTransactionById(id: string): Observable<TransactionDetail> {
    return this.http.get<TransactionDetail>(`${this.baseUrl}/${id}`);
  }

  createDispute(transactionId: string, request: CreateDisputeRequest): Observable<DisputeDetail> {
    return this.http.post<DisputeDetail>(`${this.baseUrl}/${transactionId}/disputes`, request);
  }
}

