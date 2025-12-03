using MediatR;
using VerbundPflegehilfe.Application.Common.Interfaces;
using VerbundPflegehilfe.Application.Common.Models;

namespace VerbundPflegehilfe.Application.TodoItems.Commands.DeleteTodo;

public class DeleteTodoCommandHandler(IApplicationDbContext context)
    : IRequestHandler<DeleteTodoCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoItems
            .FindAsync([request.Id], cancellationToken);

        if (entity == null)
        {
            return Result<bool>.Failure($"Todo item with ID '{request.Id}' was not found.");
        }

        context.TodoItems.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, "Task deleted successfully.");
    }
}