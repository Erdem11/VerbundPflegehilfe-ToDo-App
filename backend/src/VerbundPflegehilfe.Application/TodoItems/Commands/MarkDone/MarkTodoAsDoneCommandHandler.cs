using MediatR;
using VerbundPflegehilfe.Application.Common.Interfaces;
using VerbundPflegehilfe.Application.Common.Models;

namespace VerbundPflegehilfe.Application.TodoItems.Commands.MarkDone;

public class MarkTodoAsDoneCommandHandler(IApplicationDbContext context) 
    : IRequestHandler<MarkTodoAsDoneCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(MarkTodoAsDoneCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoItems
            .FindAsync([request.Id], cancellationToken);

        if (entity == null)
        {
            return Result<bool>.Failure($"Todo item with ID '{request.Id}' was not found.");
        }

        entity.MarkAsDone();

        await context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, "Task marked as done.");
    }
}