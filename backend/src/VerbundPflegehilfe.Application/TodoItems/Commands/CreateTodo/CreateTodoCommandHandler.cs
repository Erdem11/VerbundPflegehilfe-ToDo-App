using VerbundPflegehilfe.Application.Common.Interfaces;
using VerbundPflegehilfe.Application.Common.Models;
using VerbundPflegehilfe.Domain.Entities;
using MediatR;

namespace VerbundPflegehilfe.Application.TodoItems.Commands.CreateTodo;

public class CreateTodoCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateTodoCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem(request.Title, request.DueDate);

        context.TodoItems.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(entity.Id);
    }
}