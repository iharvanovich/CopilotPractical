import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { CoursesApiService } from '../../core/services/api';
import { Course } from '../../shared/models/course';

@Component({
  selector: 'app-courses-page',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule
  ],
  templateUrl: './courses-page.component.html',
  styleUrl: './courses-page.component.scss'
})
export class CoursesPageComponent implements OnInit {
  courses: Course[] = [];
  displayedColumns: string[] = ['title', 'courseCategoryName', 'estimatedHours', 'actions'];
  loading = true;
  error: string | null = null;

  constructor(private coursesApi: CoursesApiService) {}

  ngOnInit(): void {
    this.loadCourses();
  }

  private loadCourses(): void {
    this.coursesApi.getAll().subscribe({
      next: (data) => {
        this.courses = data;
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load courses';
        this.loading = false;
        console.error(err);
      }
    });
  }

  deleteCourse(id: string): void {
    if (!confirm('Are you sure you want to delete this course?')) return;
    
    this.coursesApi.delete(id).subscribe({
      next: () => {
        this.courses = this.courses.filter(c => c.id !== id);
      },
      error: (err: any) => {
        this.error = 'Failed to delete course';
        console.error(err);
      }
    });
  }
}
