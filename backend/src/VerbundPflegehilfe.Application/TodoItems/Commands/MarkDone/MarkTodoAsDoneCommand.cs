using MediatR;
using VerbundPflegehilfe.Application.Common.Models;

namespace VerbundPflegehilfe.Application.TodoItems.Commands.MarkDone;

public record MarkTodoAsDoneCommand(Guid Id) : IRequest<Result<bool>>;