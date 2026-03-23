export interface Course {
  id: string;
  title: string;
  description?: string;
  courseCategoryId?: string;
  courseCategoryName?: string;
  estimatedHours?: number;
}

export interface CreateCourse {
  title: string;
  description?: string;
  courseCategoryId?: string;
  estimatedHours?: number;
}

export interface UpdateCourse extends CreateCourse {
  id: string;
}

export interface CourseCategory {
  id?: string;
  name: string;
}
