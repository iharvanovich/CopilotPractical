import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CourseAssignment, CreateAssignment, UpdateAssignmentStatus } from '../../../shared/models/course-assignment';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root'
})
export class CourseAssignmentsApiService {
  constructor(private httpClient: HttpClientService) {}

  getAll(): Observable<CourseAssignment[]> {
    return this.httpClient.get<CourseAssignment[]>('courseassignments');
  }

  getById(id: string): Observable<CourseAssignment> {
    return this.httpClient.get<CourseAssignment>(`courseassignments/${id}`);
  }

  create(assignment: CreateAssignment): Observable<CourseAssignment> {
    return this.httpClient.post<CourseAssignment>('courseassignments', assignment);
  }

  updateStatus(assignment: UpdateAssignmentStatus): Observable<void> {
    return this.httpClient.patch<void>(`courseassignments/${assignment.id}/status`, assignment);
  }

  delete(id: string): Observable<void> {
    return this.httpClient.delete(`courseassignments/${id}`);
  }
}
