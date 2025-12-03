using MediatR;
using VerbundPflegehilfe.Application.Common.Interfaces;
using VerbundPflegehilfe.Application.Common.Models;

namespace VerbundPflegehilfe.Application.TodoItems.Queries.GetTodos;

public class GetTodosQueryHandler(IApplicationDbContext context) : IRequestHandler<GetTodosQuery, Result<PaginatedList<TodoItemDto>>>
{
    public async Task<Result<PaginatedList<TodoItemDto>>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        var query = context.TodoItems
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new TodoItemDto
            {
                Id = x.Id,
                Title = x.Title,
                IsCompleted = x.IsCompleted,
                DueDate = x.DueDate,
                IsOverdue = x.DueDate != null && x.DueDate.Value.Date < DateTime.UtcNow.Date && !x.IsCompleted
            });

        var paginatedList = await PaginatedList<TodoItemDto>.CreateAsync(query, request.PageNumber, request.PageSize);

        return Result<PaginatedList<TodoItemDto>>.Success(paginatedList);
    }
}