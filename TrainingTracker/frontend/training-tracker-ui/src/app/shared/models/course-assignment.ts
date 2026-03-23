import { AssignmentStatus } from './assignment-status.enum';

export interface CourseAssignment {
  id: string;
  employeeId: string;
  employeeName: string;
  courseId: string;
  courseTitle: string;
  assignedAt: Date;
  dueAt?: Date;
  completedAt?: Date;
  status: AssignmentStatus;
  notes?: string;
}

export interface CreateAssignment {
  employeeId: string;
  courseId: string;
  dueAt?: Date;
  notes?: string;
}

export interface UpdateAssignmentStatus {
  id: string;
  status: AssignmentStatus;
  completedAt?: Date;
  notes?: string;
}

export { AssignmentStatus };
