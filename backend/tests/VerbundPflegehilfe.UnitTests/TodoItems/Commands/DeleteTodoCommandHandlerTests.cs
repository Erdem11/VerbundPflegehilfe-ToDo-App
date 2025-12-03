using FluentAssertions;
using Moq;
using VerbundPflegehilfe.Application.Common.Interfaces;
using VerbundPflegehilfe.Application.TodoItems.Commands.DeleteTodo;
using VerbundPflegehilfe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace VerbundPflegehilfe.UnitTests.TodoItems.Commands;

public class DeleteTodoCommandHandlerTests
{
    [Fact]
    public async Task Handle_GivenValidId_ShouldDeleteAndReturnSuccess()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var todoItem = new TodoItem("Task to Delete", DateTime.Now);

        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<IApplicationDbContext>();

        mockSet.Setup(m => m.FindAsync(It.Is<object[]>(ids => (Guid)ids[0] == todoId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoItem);

        mockContext.Setup(m => m.TodoItems).Returns(mockSet.Object);

        var handler = new DeleteTodoCommandHandler(mockContext.Object);
        var command = new DeleteTodoCommand(todoId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.Should().BeTrue();

        mockSet.Verify(m => m.Remove(todoItem), Times.Once());

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

        var handler = new DeleteTodoCommandHandler(mockContext.Object);
        var command = new DeleteTodoCommand(Guid.NewGuid());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.Should().BeFalse();
        mockSet.Verify(m => m.Remove(It.IsAny<TodoItem>()), Times.Never());
        mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Never());
    }
}