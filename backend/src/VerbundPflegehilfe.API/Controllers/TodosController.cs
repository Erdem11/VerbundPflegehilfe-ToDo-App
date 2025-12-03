using Microsoft.AspNetCore.Mvc;
using VerbundPflegehilfe.Application.TodoItems.Commands.CreateTodo;
using VerbundPflegehilfe.Application.TodoItems.Queries.GetTodos;
using VerbundPflegehilfe.Application.Common.Models;
using VerbundPflegehilfe.Application.TodoItems.Commands.DeleteTodo;
using VerbundPflegehilfe.Application.TodoItems.Commands.MarkDone;

namespace VerbundPflegehilfe.API.Controllers;

public class TodosController : ApiClientBaseController
{
    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> Create(CreateTodoCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<TodoItemDto>>>> Get([FromQuery] GetTodosQuery query)
    {
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}/mark-done")]
    public async Task<ActionResult<Result<bool>>> MarkAsDone(Guid id)
    {
        var command = new MarkTodoAsDoneCommand(id);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<bool>>> Delete(Guid id)
    {
        var command = new DeleteTodoCommand(id);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
}