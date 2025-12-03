using FluentValidation.TestHelper;
using VerbundPflegehilfe.Application.TodoItems.Commands.CreateTodo;

namespace VerbundPflegehilfe.UnitTests.TodoItems.Commands;

public class CreateTodoCommandValidatorTests
{
    private readonly CreateTodoCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Title_Is_Too_Short()
    {
        // Arrange
        var command = new CreateTodoCommand("Short", DateTime.Now);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Title)
            .WithErrorMessage("Task must be longer than 10 characters.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Title_Is_Valid()
    {
        // Arrange
        var command = new CreateTodoCommand("This is a sufficiently long title", DateTime.Now);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Title);
    }
}