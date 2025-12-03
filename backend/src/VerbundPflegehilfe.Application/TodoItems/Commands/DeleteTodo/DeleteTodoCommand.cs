using MediatR;
using VerbundPflegehilfe.Application.Common.Models;

namespace VerbundPflegehilfe.Application.TodoItems.Commands.DeleteTodo;

public record DeleteTodoCommand(Guid Id) : IRequest<Result<bool>>;