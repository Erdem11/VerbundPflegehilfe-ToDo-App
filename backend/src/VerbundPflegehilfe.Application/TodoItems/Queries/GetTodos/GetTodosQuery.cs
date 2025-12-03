using MediatR;
using VerbundPflegehilfe.Application.Common.Models;

namespace VerbundPflegehilfe.Application.TodoItems.Queries.GetTodos;

public record GetTodosQuery : IRequest<Result<PaginatedList<TodoItemDto>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}