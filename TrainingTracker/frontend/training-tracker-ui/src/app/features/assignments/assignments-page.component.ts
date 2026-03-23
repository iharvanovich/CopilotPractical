import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatChipsModule } from '@angular/material/chips';
import { CourseAssignmentsApiService } from '../../core/services/api';
import { CourseAssignment, AssignmentStatus } from '../../shared/models/course-assignment';

@Component({
  selector: 'app-assignments-page',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatChipsModule
  ],
  templateUrl: './assignments-page.component.html',
  styleUrl: './assignments-page.component.scss'
})
export class AssignmentsPageComponent implements OnInit {
  assignments: CourseAssignment[] = [];
  displayedColumns: string[] = ['employeeName', 'courseTitle', 'assignedAt', 'status', 'actions'];
  loading = true;
  error: string | null = null;
  AssignmentStatus = AssignmentStatus;

  constructor(private assignmentsApi: CourseAssignmentsApiService) {}

  ngOnInit(): void {
    this.loadAssignments();
  }

  private loadAssignments(): void {
    this.assignmentsApi.getAll().subscribe({
      next: (data) => {
        this.assignments = data;
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load assignments';
        this.loading = false;
        console.error(err);
      }
    });
  }

  deleteAssignment(id: string): void {
    if (!confirm('Are you sure you want to delete this assignment?')) return;
    
    this.assignmentsApi.delete(id).subscribe({
      next: () => {
        this.assignments = this.assignments.filter(a => a.id !== id);
      },
      error: (err: any) => {
        this.error = 'Failed to delete assignment';
        console.error(err);
      }
    });
  }

  getStatusClass(status: AssignmentStatus): string {
    switch (status) {
      case AssignmentStatus.Pending:
        return 'status-pending';
      case AssignmentStatus.Assigned:
        return 'status-assigned';
      case AssignmentStatus.InProgress:
        return 'status-in-progress';
      case AssignmentStatus.Completed:
        return 'status-completed';
      case AssignmentStatus.Overdue:
        return 'status-overdue';
      case AssignmentStatus.Cancelled:
        return 'status-cancelled';
      default:
        return '';
    }
  }

  getStatusLabel(status: AssignmentStatus): string {
    return AssignmentStatus[status];
  }
}
