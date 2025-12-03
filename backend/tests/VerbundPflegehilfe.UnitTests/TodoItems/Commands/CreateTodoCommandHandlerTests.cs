using FluentAssertions;
using Moq;
using VerbundPflegehilfe.Application.Common.Interfaces;
using VerbundPflegehilfe.Application.TodoItems.Commands.CreateTodo;
using VerbundPflegehilfe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace VerbundPflegehilfe.UnitTests.TodoItems.Commands;

public class CreateTodoCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Success_Result_With_Id()
    {
        // Arrange
        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<IApplicationDbContext>();

        mockContext.Setup(m => m.TodoItems).Returns(mockSet.Object);

        var handler = new CreateTodoCommandHandler(mockContext.Object);
        var command = new CreateTodoCommand("Test Task Title", DateTime.Now.AddDays(1));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();

        result.Succeeded.Should().BeTrue();

        result.Message.Should().NotBeNullOrEmpty();

        result.Data.Should().NotBe(Guid.Empty);

        mockSet.Verify(m => m.Add(It.IsAny<TodoItem>()), Times.Once());
        mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
    }
}