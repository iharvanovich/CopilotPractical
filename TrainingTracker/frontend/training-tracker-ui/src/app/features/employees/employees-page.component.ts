import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { EmployeesApiService } from '../../core/services/api';
import { Employee } from '../../shared/models/employee';

@Component({
  selector: 'app-employees-page',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule
  ],
  templateUrl: './employees-page.component.html',
  styleUrl: './employees-page.component.scss'
})
export class EmployeesPageComponent implements OnInit {
  employees: Employee[] = [];
  displayedColumns: string[] = ['fullName', 'email', 'actions'];
  loading = true;
  error: string | null = null;

  constructor(private employeesApi: EmployeesApiService) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  private loadEmployees(): void {
    this.employeesApi.getAll().subscribe({
      next: (data) => {
        this.employees = data;
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load employees';
        this.loading = false;
        console.error(err);
      }
    });
  }

  deleteEmployee(id: string): void {
    if (!confirm('Are you sure you want to delete this employee?')) return;
    
    this.employeesApi.delete(id).subscribe({
      next: () => {
        this.employees = this.employees.filter(e => e.id !== id);
      },
      error: (err: any) => {
        this.error = 'Failed to delete employee';
        console.error(err);
      }
    });
  }
}
