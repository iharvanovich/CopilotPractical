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

    // Note: This is a placeholder. Implement actual create/update endpoints in the backend if needed.
    // For now, we'll just add it locally to demonstrate the UI.
    const category: CourseCategory = {
      id: this.editingId || new Date().getTime().toString(),
      name: this.form.value.name
    };

    if (this.editingId) {
      const index = this.categories.findIndex(c => c.id === this.editingId);
      if (index >= 0) {
        this.categories[index] = category;
      }
    } else {
      this.categories.push(category);
    }

    this.form.reset();
    this.editingId = null;
    this.saving = false;
  }

  editCategory(category: CourseCategory): void {
    this.form.patchValue({ name: category.name });
    this.editingId = category.id;
  }

  deleteCategory(id: string): void {
    if (!confirm('Are you sure you want to delete this category?')) return;
    this.categories = this.categories.filter(c => c.id !== id);
  }

  cancel(): void {
    this.form.reset();
    this.editingId = null;
  }
}
