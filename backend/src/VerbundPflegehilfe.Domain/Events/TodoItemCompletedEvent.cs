using VerbundPflegehilfe.Domain.Common;
using VerbundPflegehilfe.Domain.Entities;

namespace VerbundPflegehilfe.Domain.Events;

public class TodoItemCompletedEvent(TodoItem item) : BaseEvent
{
    public TodoItem Item { get; } = item;
}