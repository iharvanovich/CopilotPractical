export interface Employee {
  id: string;
  fullName: string;
  email?: string;
}

export interface CreateEmployee {
  fullName: string;
  email?: string;
}

export interface UpdateEmployee extends CreateEmployee {
  id: string;
}

export interface EmployeeDetails extends Employee {}
