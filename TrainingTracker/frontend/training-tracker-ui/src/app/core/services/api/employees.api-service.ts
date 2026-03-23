import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Employee, EmployeeDetails } from '../../../shared/models/employee';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root'
})
export class EmployeesApiService {
  constructor(private httpClient: HttpClientService) {}

  getAll(): Observable<Employee[]> {
    return this.httpClient.get<Employee[]>('employees');
  }

  getById(id: string): Observable<EmployeeDetails> {
    return this.httpClient.get<EmployeeDetails>(`employees/${id}`);
  }

  create(employee: Employee): Observable<Employee> {
    return this.httpClient.post<Employee>('employees', employee);
  }

  update(employee: Employee): Observable<void> {
    return this.httpClient.put<void>(`employees/${employee.id}`, employee);
  }

  delete(id: string): Observable<void> {
    return this.httpClient.delete(`employees/${id}`);
  }
}
