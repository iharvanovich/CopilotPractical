import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Course, CreateCourse, UpdateCourse, CourseCategory } from '../../../shared/models/course';
import { HttpClientService } from './http-client.service';

@Injectable({
  providedIn: 'root'
})
export class CoursesApiService {
  constructor(private httpClient: HttpClientService) {}

  getAll(): Observable<Course[]> {
    return this.httpClient.get<Course[]>('courses');
  }

  getById(id: string): Observable<Course> {
    return this.httpClient.get<Course>(`courses/${id}`);
  }

  create(course: CreateCourse): Observable<Course> {
    return this.httpClient.post<Course>('courses', course);
  }

  update(course: UpdateCourse): Observable<void> {
    return this.httpClient.put<void>(`courses/${course.id}`, course);
  }

  delete(id: string): Observable<void> {
    return this.httpClient.delete(`courses/${id}`);
  }

  getCategories(): Observable<CourseCategory[]> {
    return this.httpClient.get<CourseCategory[]>('coursecategories');
  }

  createCategory(category: CourseCategory): Observable<CourseCategory> {
    return this.httpClient.post<CourseCategory>('coursecategories', category);
  }

  updateCategory(category: CourseCategory): Observable<void> {
    return this.httpClient.put<void>(`coursecategories/${category.id}`, category);
  }

  deleteCategory(id: string): Observable<void> {
    return this.httpClient.delete(`coursecategories/${id}`);
  }
}
