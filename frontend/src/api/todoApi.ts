import {requests} from './agent';
import type {PaginatedList} from '../common/types';
import type {CreateTodoParams, GetTodosParams, TodoItem} from '../models/todo';

const TodoApi = {
  list: (params: GetTodosParams) => requests.get<PaginatedList<TodoItem>>('/todos', params),
  create: (todo: CreateTodoParams) => requests.post<string>('/todos', todo),
  markAsDone: (id: number) => requests.put<void>(`/todos/${id}/mark-done`, {}),
  delete: (id: number) => requests.del<void>(`/todos/${id}`),
};

export default TodoApi;