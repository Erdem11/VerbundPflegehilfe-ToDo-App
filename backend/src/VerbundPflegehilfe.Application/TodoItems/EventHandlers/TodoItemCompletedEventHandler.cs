using MediatR;
using Microsoft.Extensions.Logging;
using VerbundPflegehilfe.Domain.Events;

namespace VerbundPflegehilfe.Application.TodoItems.EventHandlers;

public class TodoItemCompletedEventHandler(ILogger<TodoItemCompletedEventHandler> logger) : INotificationHandler<TodoItemCompletedEvent>
{
    public Task Handle(TodoItemCompletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("VerbundPflegehilfe Domain Event: Task completed! Task ID: {Id}, Title: {Title}",
        notification.Item.Id, notification.Item.Title);

        return Task.CompletedTask;
    }
}