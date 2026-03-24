import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { CoursesApiService } from '../../core/services/api';
import { CourseCategory } from '../../shared/models/course';

@Component({
  selector: 'app-admin-page',
  standalone: true,
  imports: [
    CommonModule,
    MatTabsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule
  ],
  templateUrl: './admin-page.component.html',
  styleUrl: './admin-page.component.scss'
})
export class AdminPageComponent implements OnInit {
  categories: CourseCategory[] = [];
  displayedColumns: string[] = ['name', 'actions'];
  loading = true;
  saving = false;
  error: string | null = null;
  form: FormGroup;
  editingId: string | null = null;

  constructor(
    private coursesApi: CoursesApiService,
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.loadCategories();
  }

  private loadCategories(): void {
    this.loading = true;
    this.error = null;

    this.coursesApi.getCategories().subscribe({
      next: (data) => {
        this.categories = data;
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load categories';
        this.loading = false;
        console.error(err);
      }
    });
  }

  saveCategory(): void {
    if (!this.form.valid) return;

    this.saving = true;
    this.error = null;

    const category: CourseCategory = {
      id: this.editingId || undefined,
      name: this.form.value.name?.trim()
    };

    if (this.editingId) {
      this.coursesApi
        .updateCategory(category)
        .pipe(finalize(() => (this.saving = false)))
        .subscribe({
          next: () => {
            const index = this.categories.findIndex(c => c.id === this.editingId);
            if (index >= 0) {
              this.categories[index] = { ...this.categories[index], ...category };
              this.categories = [...this.categories];
            }

            this.form.reset();
            this.editingId = null;
          },
          error: (err: any) => {
            this.error = 'Failed to save category';
            console.error(err);
          }
        });
    } else {
      this.coursesApi
        .createCategory(category)
        .pipe(finalize(() => (this.saving = false)))
        .subscribe({
          next: (createdCategory) => {
            this.categories = [...this.categories, createdCategory];
            this.form.reset();
            this.editingId = null;
          },
          error: (err: any) => {
            this.error = 'Failed to save category';
            console.error(err);
          }
        });
    }
  }

  editCategory(category: CourseCategory): void {
    this.form.patchValue({ name: category.name });
    this.editingId = category.id || null;
  }

  deleteCategory(id: string): void {
    if (!confirm('Are you sure you want to delete this category?')) return;

    this.saving = true;
    this.error = null;

    this.coursesApi
      .deleteCategory(id)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe({
        next: () => {
          this.categories = this.categories.filter((c) => c.id !== id);
          // If we were editing the deleted category, reset the form
          if (this.editingId === id) {
            this.cancel();
          }
        },
        error: (err: any) => {
          this.error = 'Failed to delete category';
          console.error(err);
        }
      });
  }

  cancel(): void {
    this.form.reset();
    this.editingId = null;
  }
}
