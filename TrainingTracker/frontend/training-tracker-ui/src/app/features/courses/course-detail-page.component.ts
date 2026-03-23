import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { CoursesApiService } from '../../core/services/api';
import { Course, CourseCategory, CreateCourse, UpdateCourse } from '../../shared/models';

@Component({
  selector: 'app-course-detail-page',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatProgressBarModule,
    MatSelectModule
  ],
  templateUrl: './course-detail-page.component.html',
  styleUrl: './course-detail-page.component.scss'
})
export class CourseDetailPageComponent implements OnInit {
  form: FormGroup;
  loading = false;
  saving = false;
  error: string | null = null;
  courseId: string | null = null;
  categories: CourseCategory[] = [];

  constructor(
    private fb: FormBuilder,
    private coursesApi: CoursesApiService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.form = this.fb.group({
      title: ['', [Validators.required]],
      description: [''],
      courseCategoryId: [''],
      estimatedHours: [null]
    });
  }

  ngOnInit(): void {
    this.loadCategories();
    this.courseId = this.route.snapshot.paramMap.get('id');
    if (this.courseId && this.courseId !== 'new') {
      this.loadCourse();
    }
  }

  private loadCategories(): void {
    this.coursesApi.getCategories().subscribe({
      next: (categories) => {
        this.categories = categories;
      },
      error: (err) => {
        console.error('Failed to load categories', err);
      }
    });
  }

  private loadCourse(): void {
    if (!this.courseId) return;
    this.loading = true;
    this.coursesApi.getById(this.courseId).subscribe({
      next: (course) => {
        this.form.patchValue(course);
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load course';
        this.loading = false;
        console.error(err);
      }
    });
  }

  save(): void {
    if (!this.form.valid) return;
    this.saving = true;

    const formValue = this.form.value;
    
    if (this.courseId && this.courseId !== 'new') {
      const updateCourse: UpdateCourse = {
        id: this.courseId,
        ...formValue
      };
      this.coursesApi.update(updateCourse).subscribe({
        next: () => {
          this.router.navigate(['/courses']);
        },
        error: (err) => {
          this.error = 'Failed to save course';
          this.saving = false;
          console.error(err);
        }
      });
    } else {
      const createCourse: CreateCourse = formValue;
      this.coursesApi.create(createCourse).subscribe({
        next: () => {
          this.router.navigate(['/courses']);
        },
        error: (err) => {
          this.error = 'Failed to save course';
          this.saving = false;
          console.error(err);
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/courses']);
  }
}
