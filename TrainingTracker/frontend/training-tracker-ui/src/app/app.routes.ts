import { Routes } from '@angular/router';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { DashboardPageComponent } from './features/dashboard/dashboard-page.component';
import { EmployeesPageComponent } from './features/employees/employees-page.component';
import { EmployeeDetailPageComponent } from './features/employees/employee-detail-page.component';
import { CoursesPageComponent } from './features/courses/courses-page.component';
import { CourseDetailPageComponent } from './features/courses/course-detail-page.component';
import { AssignmentsPageComponent } from './features/assignments/assignments-page.component';
import { AssignmentDetailPageComponent } from './features/assignments/assignment-detail-page.component';
import { OverduePageComponent } from './features/overdue/overdue-page.component';
import { AdminPageComponent } from './features/admin/admin-page.component';

export const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardPageComponent },
      {
        path: 'employees',
        children: [
          { path: '', component: EmployeesPageComponent },
          { path: ':id', component: EmployeeDetailPageComponent }
        ]
      },
      {
        path: 'courses',
        children: [
          { path: '', component: CoursesPageComponent },
          { path: ':id', component: CourseDetailPageComponent }
        ]
      },
      {
        path: 'assignments',
        children: [
          { path: '', component: AssignmentsPageComponent },
          { path: ':id', component: AssignmentDetailPageComponent }
        ]
      },
      { path: 'overdue', component: OverduePageComponent },
      { path: 'admin', component: AdminPageComponent }
    ]
  }
];
