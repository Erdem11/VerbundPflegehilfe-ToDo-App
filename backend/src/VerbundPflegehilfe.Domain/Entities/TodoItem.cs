using VerbundPflegehilfe.Domain.Common;
using VerbundPflegehilfe.Domain.Events;

namespace VerbundPflegehilfe.Domain.Entities;

public class TodoItem : BaseAuditableEntity
{
    public string Title { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime? DueDate { get; private set; }

    private TodoItem() { }

    public TodoItem(string title, DateTime? dueDate)
    {
        Id = Guid.NewGuid();
        Title = title;
        IsCompleted = false;
        DueDate = dueDate;
    }

    public void MarkAsDone()
    {
        if (IsCompleted) return;

        IsCompleted = true;

        AddDomainEvent(new TodoItemCompletedEvent(this));
    }
}