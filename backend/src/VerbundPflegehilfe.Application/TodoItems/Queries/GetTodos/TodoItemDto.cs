namespace VerbundPflegehilfe.Application.TodoItems.Queries.GetTodos;

public class TodoItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsOverdue { get; set; }
}