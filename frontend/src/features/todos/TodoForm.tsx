import React from 'react';
import {useForm} from 'react-hook-form';
import {zodResolver} from '@hookform/resolvers/zod';
import {z} from 'zod';
import {Button, Form, Card, Row, Col, Spinner} from 'react-bootstrap';
import {useCreateTodo} from './useTodoQueries';
import {FaPlus} from 'react-icons/fa';

const todoSchema = z.object({
  title: z.string().min(10, "Task description must be at least 10 characters long."),
  dueDate: z.string().optional().or(z.literal('')),
});

type TodoFormInputs = z.infer<typeof todoSchema>;

const TodoForm: React.FC = () => {
  const createMutation = useCreateTodo();

  const {register, handleSubmit, reset, formState: {errors}} = useForm<TodoFormInputs>({
    resolver: zodResolver(todoSchema)
  });

  const onSubmit = (data: TodoFormInputs) => {
    const payload = {
      title: data.title,
      dueDate: data.dueDate || undefined
    };

    createMutation.mutate(payload, {
      onSuccess: () => {
        reset();
      }
    });
  };

  return (
    <Card className="shadow-sm border-0 mb-4 bg-light">
      <Card.Body>
        <h5 className="card-title mb-3 text-primary">Add New Task</h5>
        <Form onSubmit={handleSubmit(onSubmit)}>
          <Row>
            {/* Task Title Input */}
            <Col md={6}>
              <Form.Group controlId="formTitle">
                <Form.Control
                  type="text"
                  placeholder="What needs to be done? (min 10 chars)"
                  {...register("title")}
                  isInvalid={!!errors.title}
                />
                <Form.Control.Feedback type="invalid">
                  {errors.title?.message}
                </Form.Control.Feedback>
              </Form.Group>
            </Col>

            {/* Due Date Input */}
            <Col md={3}>
              <Form.Group controlId="formDate">
                <Form.Control
                  type="date"
                  {...register("dueDate")}
                />
              </Form.Group>
            </Col>

            {/* Submit Button */}
            <Col md={1} className="d-grid">
              <Button
                variant="primary"
                type="submit"
                disabled={createMutation.isPending}
              >
                {createMutation.isPending ? <Spinner size="sm"/> : <FaPlus/>}
              </Button>
            </Col>
          </Row>
        </Form>
      </Card.Body>
    </Card>
  );
};

export default TodoForm;