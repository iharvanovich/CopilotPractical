import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { EmployeesApiService } from '../../core/services/api';
import { Employee } from '../../shared/models/employee';

@Component({
  selector: 'app-employee-detail-page',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatProgressBarModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './employee-detail-page.component.html',
  styleUrl: './employee-detail-page.component.scss'
})
export class EmployeeDetailPageComponent implements OnInit {
  form: FormGroup;
  loading = false;
  saving = false;
  error: string | null = null;
  employeeId: string | null = null;

  constructor(
    private fb: FormBuilder,
    private employeesApi: EmployeesApiService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.form = this.fb.group({
      fullName: ['', [Validators.required, Validators.maxLength(200)]],
      email: ['', [Validators.email]]
    });
  }

  ngOnInit(): void {
    this.employeeId = this.route.snapshot.paramMap.get('id');
    if (this.employeeId && this.employeeId !== 'new') {
      this.loadEmployee();
    }
  }

  private loadEmployee(): void {
    if (!this.employeeId) return;
    this.loading = true;
    this.employeesApi.getById(this.employeeId).subscribe({
      next: (employee) => {
        this.form.patchValue(employee);
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load employee';
        this.loading = false;
        console.error(err);
      }
    });
  }

  save(): void {
    if (!this.form.valid) return;
    this.saving = true;

    const employee: Employee = {
      id: this.employeeId || '',
      ...this.form.value
    };

    const request$ = this.employeeId && this.employeeId !== 'new'
      ? this.employeesApi.update(employee) as any
      : this.employeesApi.create(employee) as any;

    request$.subscribe({
      next: () => {
        this.router.navigate(['/employees']);
      },
      error: (err: any) => {
        this.error = 'Failed to save employee';
        this.saving = false;
        console.error(err);
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/employees']);
  }
}
