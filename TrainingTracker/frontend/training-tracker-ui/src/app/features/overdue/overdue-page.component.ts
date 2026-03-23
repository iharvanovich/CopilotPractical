import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatChipsModule } from '@angular/material/chips';
import { CourseAssignmentsApiService } from '../../core/services/api';
import { CourseAssignment, AssignmentStatus } from '../../shared/models/course-assignment';

@Component({
  selector: 'app-overdue-page',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatProgressBarModule,
    MatChipsModule
  ],
  templateUrl: './overdue-page.component.html',
  styleUrl: './overdue-page.component.scss'
})
export class OverduePageComponent implements OnInit {
  overdueAssignments: CourseAssignment[] = [];
  displayedColumns: string[] = ['employeeName', 'courseTitle', 'assignedAt', 'dueAt', 'daysOverdue'];
  loading = true;
  error: string | null = null;
  AssignmentStatus = AssignmentStatus;

  constructor(private assignmentsApi: CourseAssignmentsApiService) {}

  ngOnInit(): void {
    this.loadOverdueAssignments();
  }

  private loadOverdueAssignments(): void {
    this.assignmentsApi.getAll().subscribe({
      next: (data) => {
        this.overdueAssignments = data.filter(a => a.status === AssignmentStatus.Overdue);
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load overdue assignments';
        this.loading = false;
        console.error(err);
      }
    });
  }

  getDaysOverdue(dueAt?: Date): number {
    if (!dueAt) return 0;
    const dueDate = new Date(dueAt);
    const today = new Date();
    const diffTime = today.getTime() - dueDate.getTime();
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    return diffDays;
  }
}
