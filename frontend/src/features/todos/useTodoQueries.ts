import {useQuery, useMutation, useQueryClient} from '@tanstack/react-query';
import TodoApi from '../../api/todoApi';
import type {CreateTodoParams, GetTodosParams} from '../../models/todo';
import {toast} from 'react-toastify';

export const useTodos = (params: GetTodosParams) => {
  return useQuery({
    queryKey: ['todos', params.pageNumber, params.pageSize],
    queryFn: () => TodoApi.list(params),
  });
};

export const useCreateTodo = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (todo: CreateTodoParams) => TodoApi.create(todo),
    onSuccess: async () => {
      toast.success('Task created successfully!');
      await queryClient.invalidateQueries({queryKey: ['todos']});
    }
  });
};

export const useMarkTodoAsDone = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: number) => TodoApi.markAsDone(id),
    onSuccess: async () => {
      toast.success('Task marked as done!');
      await queryClient.invalidateQueries({queryKey: ['todos']});
    }
  });
};

export const useDeleteTodo = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: number) => TodoApi.delete(id),
    onSuccess: async () => {
      toast.success('Task deleted!');
      await queryClient.invalidateQueries({queryKey: ['todos']});
    }
  });
};