import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DashboardSummary } from '../../../shared/models/dashboard-summary';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root'
})
export class DashboardApiService {
  constructor(private httpClient: HttpClientService) {}

  getSummary(): Observable<DashboardSummary> {
    return this.httpClient.get<DashboardSummary>('dashboard');
  }
}
