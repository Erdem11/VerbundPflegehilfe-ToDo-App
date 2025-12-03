export interface TodoItem {
  id: number;
  title: string;
  dueDate?: string;
  isCompleted: boolean;
  isOverdue: boolean;
}

export interface CreateTodoParams {
  title: string;
  dueDate?: string;
}

export interface GetTodosParams {
  pageNumber: number;
  pageSize: number;
}