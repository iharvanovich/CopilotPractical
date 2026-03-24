# CopilotPractical

# Project: Training Course Assignment Tracker
## Overview:
Training Course Assignment Tracker is a web application for assigning learning courses to employees, monitoring progress, and identifying overdue training tasks.

## Features
- Employee list and details
- Course catalog management
- Course assignment workflow
- Assignment status tracking
- Overdue training view
- Dashboard summary
- Separate main and admin layouts

## Tools Used
- GitHub Copilot Agent
- GitHub Copilot Chat
  (GPT-5 mini)
- Visual Studio / VS Code
- Angular CLI
- .NET CLI

## Prompt History
    backend:
1. Generate the Domain layer in the TrainingTracker.Domain project.
Backend projects:
- TrainingTracker.Api
- TrainingTracker.Application
- TrainingTracker.Domain
- TrainingTracker.Infrastructure
  - Create:
- Employee
- Course
- CourseCategory
- CourseAssignment
- AssignmentStatus enum
  - Requirements:
- place entities in an Entities folder
- place AssignmentStatus in an Enums folder
- make entities suitable for EF Core

2. Generate the EF Core infrastructure in the TrainingTracker.Infrastructure project.
    - Create:
- AppDbContext
- DbSet properties for Employee, Course, CourseCategory, and CourseAssignment
    - Requirements:
- use SQLite

3. Generate seed data logic in TrainingTracker.Infrastructure.
    - Create realistic seed data for:
- 6 employees
- 4 course categories
- 8 courses
- 10 course assignments
    - Requirements:
- use extension classes for Configure SQLite connection

4. Need refactor, creare base entity with field "id" and inherit from this
5. Create auditableEntity with fields CreatedAt, UpdatedAt and include in to the inheritance

6. Generate service interfaces in the TrainingTracker.Application project.
    - Create:
-	IEmployeeService
-	ICourseService
-	ICourseCategoryService
-	ICourseAssignmentService
-	IDashboardService
    - Requirements:
-	use async methods
-	return Modelss, not entities

7. please dont use " = default"

8. Generate application services in the TrainingTracker.Application project.
    - Create implementations for:
-	EmployeeService
-	CourseService
-	CourseCategoryService
-	CourseAssignmentService
-	DashboardService
    - Requirements:
-	use async methods
-	keep business logic in services
-	include overdue detection in course assignment logic
-	use generic repository

9. Update DatabaseInitializer using auditable entity
10. Create dependency injection foe application layer

11. Generate ASP.NET Core Web API controllers in TrainingTracker.Api.
    - Create:
- EmployeesController
- CoursesController
- CourseCategoriesController
- CourseAssignmentsController
- DashboardController
    - Requirements:
- controllers must stay thin
- use dependency injection
- call Application services
- return proper HTTP status codes
- use clear REST-friendly routes

12. create database migration
13. remove database migration
14. make tis field public AssignmentStatus Status { get; set; } nullable and fix usages
15. enable CORS 
16. Generate backend unit tests for the TrainingTracker solution.
    Focus on: Services
    Requirements: use xUnit
    unit tests was generated only for EmployeService
    16.1. Generate backend unit tests for the /services from application

-----------------------------------------------------------------------------------------------------
    frontend
    (open in VS Code folder with 2 projects: backend and frontend)
1. Project: Training Course Assignment Tracker
    - Repository structure:
- backend/TrainingTracker.sln
- backend/src/TrainingTracker.Api
- backend/src/TrainingTracker.Application
- backend/src/TrainingTracker.Domain
- backend/src/TrainingTracker.Infrastructure
- frontend/training-tracker-ui
    - Important backend context:
- The backend already exists in this repository
- Use backend/src/TrainingTracker.Application Models as the source of truth for data contracts
- Use backend/src/TrainingTracker.Api controllers as the source of truth for API endpoints
- Align frontend models and API services with the existing backend implementation
    - Angular requirements:
- use standalone components
- use Angular Material

************************************
I first tried without "naming conventions", and the agent generated very bad code
************************************
    - Angular naming conventions: 
- page containers must use the suffix PageComponent
- reusable UI components must use the suffix Component
- layout components must use the suffix LayoutComponent
- API services must use the suffix ApiService
- local helper services must use the suffix Service
- do not omit the Component suffix for Angular components
- keep class names, file names, and selectors consistent
    - Use domain names consistently:
- Employee
- Course
- CourseCategory
- CourseAssignment

2. enable Material Icons for index.html and styles.scss
3. use matListItemIcon and matListItemTitle for menu
4. implement on click to "Training Tracker" open dashboard
5. please implement saveCategory for use real backend
6. fix save employee but in current implementation we send id=new its incorrect
7. make colors for statuses in page Assignments
8. change color for "edit" icons on blue and "delete" icons on red (agent implemented only for one page - admin)
    8.1. update all pages when we use this icons
    8.2 please use approach globally as in admin page

9. fix #sym:deleteCategory using api call

### What worked well
- Asking for architecture first
- Generating one feature at a time
- Using explicit constraints in prompts

### What worked less well
- Asking for too much functionality in one prompt
- Working with Angular, fix scss

### Prompting Patterns That Worked Best
- Architecture first, implementation second
- Backend and frontend generated in separate steps