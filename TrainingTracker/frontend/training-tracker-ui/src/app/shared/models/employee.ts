export interface Employee {
  id: string;
  fullName: string;
  email?: string;
}

export interface EmployeeDetails extends Employee {}
