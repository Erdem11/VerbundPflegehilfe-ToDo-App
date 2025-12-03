import React, {useState} from 'react';
import {Table, Button, Badge, Spinner, Alert} from 'react-bootstrap';
import {FaCheck, FaTrash, FaExclamationCircle} from 'react-icons/fa';
import {useTodos, useDeleteTodo, useMarkTodoAsDone} from './useTodoQueries';

const TodoTable: React.FC = () => {
  const [pageNumber, setPageNumber] = useState(1);
  const pageSize = 10;

  const {data, isLoading, isError, isFetching} = useTodos({pageNumber, pageSize});

  const deleteMutation = useDeleteTodo();
  const completeMutation = useMarkTodoAsDone();

  if (isLoading) {
    return (
      <div className="text-center p-5">
        <Spinner animation="border" variant="primary"/>
        <p className="mt-2 text-muted">Loading tasks...</p>
      </div>
    );
  }

  if (isError) {
    return <Alert variant="danger">Error loading tasks. Please try again later.</Alert>;
  }

  const handleDelete = (id: number) => {
    if (window.confirm('Are you sure you want to delete this task?')) {
      deleteMutation.mutate(id);
    }
  };

  const handleComplete = (id: number) => {
    completeMutation.mutate(id);
  };

  return (
    <div className="mt-4">
      <div style={{opacity: isFetching ? 0.5 : 1, transition: 'opacity 0.2s'}}>
        <Table hover responsive bordered className="align-middle shadow-sm">
          <thead className="table-light">
          <tr>
            <th>Status</th>
            <th>Task Description</th>
            <th>Due Date</th>
            <th className="text-end">Actions</th>
          </tr>
          </thead>
          <tbody>
          {data?.items.map((todo) => {
            let rowClass = '';
            if (todo.isCompleted) rowClass = 'table-success text-muted';
            else if (todo.isOverdue) rowClass = 'table-danger';

            return (
              <tr key={todo.id} className={rowClass}>
                {/* Status Badge */}
                <td className="text-center" style={{width: '50px'}}>
                  {todo.isCompleted ? (
                    <Badge bg="success">Done</Badge>
                  ) : todo.isOverdue ? (
                    <Badge bg="danger">Overdue</Badge>
                  ) : (
                    <Badge bg="secondary">Pending</Badge>
                  )}
                </td>

                {/* Task Title */}
                <td>
                  <span style={{textDecoration: todo.isCompleted ? 'line-through' : 'none'}}>
                    {todo.title}
                  </span>
                  {todo.isOverdue && !todo.isCompleted && (
                    <small className="d-block text-danger">
                      <FaExclamationCircle className="me-1"/> Deadline missed
                    </small>
                  )}
                </td>

                {/* Due Date */}
                <td>
                  {todo.dueDate ? new Date(todo.dueDate).toLocaleDateString() : '-'}
                </td>

                {/* Action Buttons */}
                <td className="text-end">
                  {!todo.isCompleted && (
                    <Button
                      variant="outline-success"
                      size="sm"
                      className="me-2"
                      onClick={() => handleComplete(todo.id)}
                      disabled={completeMutation.isPending}
                    >
                      <FaCheck/> Done
                    </Button>
                  )}
                  <Button
                    variant="outline-danger"
                    size="sm"
                    onClick={() => handleDelete(todo.id)}
                    disabled={deleteMutation.isPending}
                  >
                    <FaTrash/> Delete
                  </Button>
                </td>
              </tr>
            );
          })}

          {/* No Tasks Found */}
          {data?.items.length === 0 && (
            <tr>
              <td colSpan={4} className="text-center py-4 text-muted">
                No tasks found. Start by creating one!
              </td>
            </tr>
          )}
          </tbody>
        </Table>
      </div>

      {/* Pagination Controls */}
      {data && (
        <div className="d-flex justify-content-between align-items-center mt-3">
          <span className="text-muted">
              Page {data.pageNumber} of {data.totalPages} (Total: {data.totalCount})
          </span>
          <div>
            <Button
              variant="secondary"
              size="sm"
              className="me-2"
              onClick={() => setPageNumber(old => Math.max(old - 1, 1))}
              disabled={!data.hasPreviousPage}
            >
              Previous
            </Button>
            <Button
              variant="secondary"
              size="sm"
              onClick={() => setPageNumber(old => old + 1)}
              disabled={!data.hasNextPage}
            >
              Next
            </Button>
          </div>
        </div>
      )}
    </div>
  );
};

export default TodoTable;