import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { DisputeDetail, DisputeListItem } from '../models/dispute.model';

@Injectable({
  providedIn: 'root'
})
export class DisputesService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/disputes';

  getDisputes(
    status?: string,
    fromDate?: string,
    toDate?: string
  ): Observable<DisputeListItem[]> {
    let params = new HttpParams();

    if (status && status.trim() && status !== 'All') {
      params = params.set('status', status.trim());
    }

    if (fromDate) {
      params = params.set('fromDate', fromDate);
    }

    if (toDate) {
      params = params.set('toDate', toDate);
    }

    return this.http.get<DisputeListItem[]>(this.baseUrl, { params });
  }

  getDisputeById(id: string): Observable<DisputeDetail> {
    return this.http.get<DisputeDetail>(`${this.baseUrl}/${id}`);
  }
}

