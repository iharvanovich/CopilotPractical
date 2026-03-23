# Training Tracker UI - Angular Application Structure

This document describes the Angular application structure for the Training Course Assignment Tracker frontend.

## Project Overview

- **Framework**: Angular 21+ with standalone components
- **UI Library**: Angular Material
- **Build Tool**: Angular CLI
- **Package Manager**: npm

## Directory Structure

```
src/app/
├── core/
│   ├── services/
│   │   ├── api/
│   │   │   ├── http-client.service.ts        # HTTP utility service
│   │   │   ├── dashboard.api-service.ts      # Dashboard API
│   │   │   ├── employees.api-service.ts      # Employee API
│   │   │   ├── courses.api-service.ts        # Course API
│   │   │   ├── course-assignments.api-service.ts  # Assignment API
│   │   │   └── index.ts                      # Barrel export
│   │   └── state/                            # State management (future use)
│   └── interceptors/                         # HTTP interceptors (future use)
├── shared/
│   ├── components/                           # Reusable UI components
│   ├── models/
│   │   ├── assignment-status.enum.ts         # Assignment status enumeration
│   │   ├── employee.ts                       # Employee model interfaces
│   │   ├── course.ts                         # Course model interfaces
│   │   ├── course-assignment.ts              # Assignment model interfaces
│   │   ├── dashboard-summary.ts              # Dashboard summary model
│   │   └── index.ts                          # Barrel export
│   └── utilities/                            # Helper functions and pipes
├── layouts/
│   ├── main-layout/                          # Main application layout
│   │   ├── main-layout.component.ts
│   │   ├── main-layout.component.html
│   │   └── main-layout.component.scss
│   └── admin-layout/                         # Admin section layout
│       ├── admin-layout.component.ts
│       ├── admin-layout.component.html
│       └── admin-layout.component.scss
├── features/
│   ├── dashboard/
│   │   ├── dashboard-page.component.ts
│   │   ├── dashboard-page.component.html
│   │   └── dashboard-page.component.scss
│   ├── employees/
│   │   ├── employees-page.component.ts       # Employee list
│   │   ├── employees-page.component.html
│   │   ├── employees-page.component.scss
│   │   ├── employee-detail-page.component.ts # Employee detail/create/edit
│   │   ├── employee-detail-page.component.html
│   │   └── employee-detail-page.component.scss
│   ├── courses/
│   │   ├── courses-page.component.ts         # Course list
│   │   ├── courses-page.component.html
│   │   ├── courses-page.component.scss
│   │   ├── course-detail-page.component.ts   # Course detail/create/edit
│   │   ├── course-detail-page.component.html
│   │   └── course-detail-page.component.scss
│   ├── assignments/
│   │   ├── assignments-page.component.ts     # Assignment list
│   │   ├── assignments-page.component.html
│   │   ├── assignments-page.component.scss
│   │   ├── assignment-detail-page.component.ts  # Assignment detail/create/edit
│   │   ├── assignment-detail-page.component.html
│   │   └── assignment-detail-page.component.scss
│   ├── overdue/
│   │   ├── overdue-page.component.ts         # Overdue assignments view
│   │   ├── overdue-page.component.html
│   │   └── overdue-page.component.scss
│   └── admin/
│       ├── admin-page.component.ts           # Admin console
│       ├── admin-page.component.html
│       └── admin-page.component.scss
├── app.ts                                    # Root component
├── app.html                                  # Root template
├── app.scss                                  # Root styles
├── app.routes.ts                             # Application routes
├── app.config.ts                             # Application configuration
└── app.spec.ts                               # Component tests
```

## Key Architectural Patterns

### 1. **Standalone Components**
All components use Angular's standalone API. There are no NgModules in this application.

### 2. **Feature-Based Organization**
Features are organized in their own folders with components, services, and models colocated.

### 3. **Naming Conventions**
- **Page Components**: Use the `PageComponent` suffix (e.g., `EmployeesPageComponent`)
- **UI Components**: Use the `Component` suffix (e.g., `ButtonComponent`)
- **Layout Components**: Use the `LayoutComponent` suffix (e.g., `MainLayoutComponent`)
- **Services**: Use the `Service` suffix (e.g., `EmployeeService`)
- **API Services**: Use the `ApiService` suffix (e.g., `EmployeesApiService`)
- **Route Guards**: Use the `Guard` suffix (e.g., `AuthGuard`)

### 4. **API Services**
All HTTP API calls are encapsulated in API services located in `core/services/api/`. Each domain has its own API service:
- `DashboardApiService` - Dashboard summary endpoint
- `EmployeesApiService` - Employee CRUD operations
- `CoursesApiService` - Course CRUD operations and category management
- `CourseAssignmentsApiService` - Assignment CRUD operations

### 5. **Models and Interfaces**
All TypeScript interfaces are defined in `shared/models/`, organized by domain:
- `Employee`, `EmployeeDetails`
- `Course`, `CreateCourse`, `UpdateCourse`, `CourseCategory`
- `CourseAssignment`, `CreateAssignment`, `UpdateAssignmentStatus`
- `DashboardSummary`
- `AssignmentStatus` enum

### 6. **Layouts**
Two main layouts are provided:
- **MainLayoutComponent**: Includes navigation sidebar and toolbar for main application features
- **AdminLayoutComponent**: Simple layout for admin section

## Routing Structure

The application uses lazy loading for better performance:

```
/ (MainLayoutComponent)
├── /dashboard (DashboardPageComponent)
├── /employees
│   ├── / (EmployeesPageComponent)
│   └── /:id (EmployeeDetailPageComponent)
├── /courses
│   ├── / (CoursesPageComponent)
│   └── /:id (CourseDetailPageComponent)
├── /assignments
│   ├── / (AssignmentsPageComponent)
│   └── /:id (AssignmentDetailPageComponent)
├── /overdue (OverduePageComponent)
└── /admin (AdminPageComponent)
```

## Data Models

### Employee
- `id`: unique identifier (GUID)
- `fullName`: employee name (required, max 200 chars)
- `email`: email address (optional)

### Course
- `id`: unique identifier (GUID)
- `title`: course title (required)
- `description`: course description (optional)
- `courseCategoryId`: category identifier (optional)
- `courseCategoryName`: category name (optional)
- `estimatedHours`: estimated duration (optional)

### CourseAssignment
- `id`: unique identifier (GUID)
- `employeeId`: assigned employee
- `employeeName`: employee name
- `courseId`: assigned course
- `courseTitle`: course title
- `assignedAt`: assignment date
- `dueAt`: due date (optional)
- `completedAt`: completion date (optional)
- `status`: assignment status (enum: Pending, Assigned, InProgress, Completed, Overdue, Cancelled)
- `notes`: assignment notes (optional)

### AssignmentStatus Enum
- `Pending` (0)
- `Assigned` (1)
- `InProgress` (2)
- `Completed` (3)
- `Overdue` (4)
- `Cancelled` (5)

### DashboardSummary
- `totalEmployees`: total employee count
- `totalCourses`: total course count
- `totalAssignments`: total assignment count
- `assignedCount`: count of assigned assignments
- `inProgressCount`: count of in-progress assignments
- `completedCount`: count of completed assignments
- `overdueCount`: count of overdue assignments

## API Configuration

The HTTP client service uses `http://localhost:5000/api` as the base URL. Update this in `core/services/api/http-client.service.ts` if your backend API is running on a different address.

## Getting Started

### Installation
```bash
npm install
```

### Development Server
```bash
npm start
# or
ng serve
```

The application will be available at `http://localhost:4200`

### Building for Production
```bash
npm run build
# or
ng build --configuration production
```

### Running Tests
```bash
npm test
# or
ng test
```

## Material Design

The application uses Angular Material components with a custom theme defined in `src/styles.scss`. The primary color is Indigo, accent is Pink, and warn is Red.

### Commonly Used Material Components
- `MatToolbarModule` - Navigation toolbar
- `MatSidenavModule` - Sidebar navigation
- `MatTableModule` - Data tables
- `MatFormFieldModule` - Form fields
- `MatButtonModule` - Buttons
- `MatIconModule` - Material icons
- `MatCardModule` - Card containers
- `MatSelectModule` - Select dropdowns
- `MatDatepickerModule` - Date picker
- `MatProgressBarModule` - Loading bar
- `MatChipsModule` - Status chips

## Form Handling

All forms use Reactive Forms with FormBuilder for better type safety and testability.

Example:
```typescript
form = this.fb.group({
  fullName: ['', [Validators.required, Validators.maxLength(200)]],
  email: ['', [Validators.email]]
});
```

## Error Handling

Each component has error handling with user-friendly error messages displayed above the main content.

## State Management

Currently, state is managed locally within components. For a larger application, consider:
- NgRx for centralized state management
- Akita as an alternative state management solution
- RxJS signals for reactive state

## Performance Considerations

1. **Lazy Loading**: Routes can be lazy loaded if needed
2. **Change Detection**: Uses OnPush strategy for better performance (can be implemented)
3. **HTTP Caching**: Can be added via interceptors
4. **Bundle Size**: Using standalone components reduces bundle size

## Future Enhancements

- [ ] Add route guards for protection
- [ ] Implement centralized state management (NgRx/Akita)
- [ ] Add comprehensive error handling interceptor
- [ ] Implement HTTP caching strategies
- [ ] Add unit and e2e tests
- [ ] Add loading states and skeleton loaders
- [ ] Implement pagination for large datasets
- [ ] Add search and filter capabilities
- [ ] Add user authentication
- [ ] Add role-based access control
- [ ] Implement toast notifications
- [ ] Add dark mode theme

## Dependencies

Core dependencies (as per package.json):
- `@angular/core`, `@angular/common`, `@angular/router`, `@angular/forms`
- `@angular/material` - UI component library
- `@angular/cdk` - Overlay providers and utilities
- `rxjs` - Reactive programming library

## Notes for Development

1. **Backend Alignment**: This frontend is designed to work with the existing backend API. Use the Application Models as the source of truth for data contracts.

2. **No Authentication**: The current implementation doesn't include authentication. Add guards and interceptors as needed for your security requirements.

3. **Admin Section**: The admin section is a lightweight frontend management area for reference data like course categories. No backend admin roles are assumed.

4. **Consistency**: All domain names (Employee, Course, CourseCategory, CourseAssignment) should be used consistently throughout the application.
