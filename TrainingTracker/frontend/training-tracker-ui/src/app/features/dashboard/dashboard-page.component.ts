import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { finalize } from 'rxjs';
import { DashboardApiService } from '../../core/services/api';
import { DashboardSummary } from '../../shared/models/dashboard-summary';

@Component({
  selector: 'app-dashboard-page',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatProgressBarModule],
  templateUrl: './dashboard-page.component.html',
  styleUrl: './dashboard-page.component.scss'
})
export class DashboardPageComponent implements OnInit {
  summary: DashboardSummary | null = null;
  loading = true;
  error: string | null = null;

  constructor(private dashboardApi: DashboardApiService) {}

  ngOnInit(): void {
    this.loadSummary();
  }

  private loadSummary(): void {
    this.loading = true;
    this.error = null;

    this.dashboardApi
      .getSummary()
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: (data) => {
          this.summary = data;
        },
        error: (err: any) => {
          this.summary = null;
          this.error = 'Failed to load dashboard summary';
          console.error(err);
        }
      });
  }

  getCompletionPercentage(): number {
    if (!this.summary || this.summary.totalAssignments === 0) return 0;
    return Math.round((this.summary.completedCount / this.summary.totalAssignments) * 100);
  }
}
