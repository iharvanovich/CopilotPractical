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
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { CourseAssignmentsApiService, EmployeesApiService, CoursesApiService } from '../../core/services/api';
import { CourseAssignment, UpdateAssignmentStatus, AssignmentStatus } from '../../shared/models/course-assignment';
import { Employee } from '../../shared/models/employee';
import { Course } from '../../shared/models/course';

@Component({
  selector: 'app-assignment-detail-page',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatProgressBarModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatIconModule
  ],
  templateUrl: './assignment-detail-page.component.html',
  styleUrl: './assignment-detail-page.component.scss'
})
export class AssignmentDetailPageComponent implements OnInit {
  form: FormGroup;
  statusForm: FormGroup;
  loading = false;
  saving = false;
  error: string | null = null;
  assignmentId: string | null = null;
  employees: Employee[] = [];
  courses: Course[] = [];
  assignment: CourseAssignment | null = null;
  AssignmentStatus = AssignmentStatus;
  assignmentStatuses = Object.values(AssignmentStatus).filter(v => typeof v === 'number');

  constructor(
    private fb: FormBuilder,
    private assignmentsApi: CourseAssignmentsApiService,
    private employeesApi: EmployeesApiService,
    private coursesApi: CoursesApiService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.form = this.fb.group({
      employeeId: ['', [Validators.required]],
      courseId: ['', [Validators.required]],
      dueAt: [''],
      notes: ['']
    });

    this.statusForm = this.fb.group({
      status: [AssignmentStatus.Assigned, [Validators.required]],
      completedAt: [''],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.loadEmployees();
    this.loadCourses();
    this.assignmentId = this.route.snapshot.paramMap.get('id');
    if (this.assignmentId && this.assignmentId !== 'new') {
      this.loadAssignment();
    }
  }

  private loadEmployees(): void {
    this.employeesApi.getAll().subscribe({
      next: (employees) => {
        this.employees = employees;
      },
      error: (err: any) => {
        console.error('Failed to load employees', err);
      }
    });
  }

  private loadCourses(): void {
    this.coursesApi.getAll().subscribe({
      next: (courses) => {
        this.courses = courses;
      },
      error: (err: any) => {
        console.error('Failed to load courses', err);
      }
    });
  }

  private loadAssignment(): void {
    if (!this.assignmentId) return;
    this.loading = true;
    this.assignmentsApi.getById(this.assignmentId).subscribe({
      next: (assignment) => {
        this.assignment = assignment;
        this.statusForm.patchValue({
          status: assignment.status,
          completedAt: assignment.completedAt,
          notes: assignment.notes
        });
        this.form.disable();
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load assignment';
        this.loading = false;
        console.error(err);
      }
    });
  }

  save(): void {
    if (!this.form.valid) return;
    this.saving = true;

    this.assignmentsApi.create(this.form.value).subscribe({
      next: () => {
        this.router.navigate(['/assignments']);
      },
      error: (err: any) => {
        this.error = 'Failed to save assignment';
        this.saving = false;
        console.error(err);
      }
    });
  }

  updateStatus(): void {
    if (!this.statusForm.valid || !this.assignmentId) return;
    this.saving = true;

    const update: UpdateAssignmentStatus = {
      id: this.assignmentId,
      ...this.statusForm.value
    };

    this.assignmentsApi.updateStatus(update).subscribe({
      next: () => {
        this.router.navigate(['/assignments']);
      },
      error: (err: any) => {
        this.error = 'Failed to update assignment status';
        this.saving = false;
        console.error(err);
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/assignments']);
  }

  getStatusLabel(status: number): string {
    return AssignmentStatus[status];
  }
}
