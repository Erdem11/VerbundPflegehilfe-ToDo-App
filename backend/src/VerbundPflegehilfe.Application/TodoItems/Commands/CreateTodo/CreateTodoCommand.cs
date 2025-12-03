using VerbundPflegehilfe.Application.Common.Models;

namespace VerbundPflegehilfe.Application.TodoItems.Commands.CreateTodo;

using MediatR;

public record CreateTodoCommand(string Title, DateTime? DueDate) : IRequest<Result<Guid>>;