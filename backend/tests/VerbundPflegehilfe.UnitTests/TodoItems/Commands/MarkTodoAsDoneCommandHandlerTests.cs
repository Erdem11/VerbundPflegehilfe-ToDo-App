using FluentAssertions;
using Moq;
using VerbundPflegehilfe.Application.Common.Interfaces;
using VerbundPflegehilfe.Application.TodoItems.Commands.MarkDone;
using VerbundPflegehilfe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace VerbundPflegehilfe.UnitTests.TodoItems.Commands;

public class MarkTodoAsDoneCommandHandlerTests
{
    [Fact]
    public async Task Handle_GivenValidId_ShouldMarkAsDoneAndReturnSuccess()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var todoItem = new TodoItem("Test Task", DateTime.UtcNow);

        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<IApplicationDbContext>();

        mockSet.Setup(m => m.FindAsync(It.Is<object[]>(ids => (Guid)ids[0] == todoId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoItem);

        mockContext.Setup(m => m.TodoItems).Returns(mockSet.Object);

        var handler = new MarkTodoAsDoneCommandHandler(mockContext.Object);
        var command = new MarkTodoAsDoneCommand(todoId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.Should().BeTrue();
        todoItem.IsCompleted.Should().BeTrue("The todo item should be marked as done.");

        mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
    }

    [Fact]
    public async Task Handle_GivenInvalidId_ShouldReturnFailure()
    {
        // Arrange
        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<IApplicationDbContext>();

        mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoItem?)null);

        mockContext.Setup(m => m.TodoItems).Returns(mockSet.Object);

        var handler = new MarkTodoAsDoneCommandHandler(mockContext.Object);
        var command = new MarkTodoAsDoneCommand(Guid.NewGuid());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Contains("was not found"));

        mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Never());
    }
}