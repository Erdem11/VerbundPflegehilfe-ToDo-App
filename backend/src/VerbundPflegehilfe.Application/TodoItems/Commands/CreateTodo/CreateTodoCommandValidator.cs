namespace VerbundPflegehilfe.Application.TodoItems.Commands.CreateTodo;

using FluentValidation;

public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(v => v.Title)
            .MinimumLength(10).WithMessage("Task must be longer than 10 characters.")
            .NotEmpty();
    }
}