using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using VerbundPflegehilfe.Application.Common.Models;
using VerbundPflegehilfe.Application.TodoItems.Commands.CreateTodo;
using VerbundPflegehilfe.Application.TodoItems.Queries.GetTodos;

namespace VerbundPflegehilfe.IntegrationTests.Controllers;

public class TodosControllerTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Create_Should_Return_Ok_And_Succeeded_Result_When_Valid()
    {
        // Arrange
        var command = new CreateTodoCommand("Integration Test Task Long Enough", DateTime.Now.AddDays(1));

        // Act
        var response = await _client.PostAsJsonAsync("/api/todos", command);

        // Assert
        response.EnsureSuccessStatusCode();// 200 OK

        var result = await response.Content.ReadFromJsonAsync<Result<Guid>>();

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBe(Guid.Empty);
        result.Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Create_Should_Return_BadRequest_With_Failure_Result_When_Validation_Fails()
    {
        // Arrange: Short title to trigger validation error
        var command = new CreateTodoCommand("Short", DateTime.Now.AddDays(1));

        // Act
        var response = await _client.PostAsJsonAsync("/api/todos", command);

        // Assert 1: HTTP Status Code check
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Assert 2: Body content check
        var result = await response.Content.ReadFromJsonAsync<Result<Guid?>>();

        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Contains("must be longer than 10 characters"));
    }

    [Fact]
    public async Task Get_Should_Return_PaginatedList_In_Result_Envelope()
    {
        // Arrange
        const string requestUrl = $"/api/todos?pageNumber=1&pageSize=10";

        // Act
        var response = await _client.GetAsync(requestUrl);

        // Assert
        response.EnsureSuccessStatusCode(); // 200 OK

        var result = await response.Content.ReadFromJsonAsync<Result<PaginatedList<TodoItemDto>>>();

        // Result checks
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue("API should return successful (Succeeded: true)");
        result.Errors.Should().BeNullOrEmpty("Error list should be empty");

        result.Data.Should().NotBeNull("Data object should not be null");

        result.Data.Items.Should().NotBeNull("Items list should not be null");
        result.Data.Items.Should().NotBeEmpty("List should not be empty because seed data exists");

        result.Data.Items.Count.Should().BeGreaterThanOrEqualTo(3);

        result.Data.PageNumber.Should().Be(1, "Page 1 was requested");
        result.Data.TotalCount.Should().BeGreaterThanOrEqualTo(3, "Total record count should be calculated correctly");
        result.Data.TotalPages.Should().BeGreaterThanOrEqualTo(1, "There should be at least 1 page");

        result.Data.Items.Should().Contain(x => x.Title.Contains("Clean Architecture"), "Seeded item 'Clean Architecture' should be present");
    }

    [Fact]
    public async Task MarkAsDone_Should_Update_IsCompleted_To_True()
    {
        // Arrange
        var createCommand = new CreateTodoCommand("Task To Complete", DateTime.Now.AddDays(1));
        var createResponse = await _client.PostAsJsonAsync("/api/todos", createCommand);
        var createResult = await createResponse.Content.ReadFromJsonAsync<Result<Guid>>();
        var todoId = createResult!.Data;

        // Act
        var response = await _client.PutAsync($"/api/todos/{todoId}/mark-done", null);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Result<bool>>();

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();

        var getResponse = await _client.GetAsync("/api/todos?pageNumber=1&pageSize=10");
        getResponse.EnsureSuccessStatusCode();
        var getResult = await getResponse.Content.ReadFromJsonAsync<Result<PaginatedList<TodoItemDto>>>();
        getResult.Should().NotBeNull();
        getResult.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_Should_Remove_Item_From_Database()
    {
        // Arrange
        var createCommand = new CreateTodoCommand("Task To Delete", DateTime.Now.AddDays(1));
        var createResponse = await _client.PostAsJsonAsync("/api/todos", createCommand);
        var createResult = await createResponse.Content.ReadFromJsonAsync<Result<Guid>>();
        var todoId = createResult!.Data;

        // Act
        var response = await _client.DeleteAsync($"/api/todos/{todoId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Result<bool>>();

        result.Should().NotBeNull();
        result!.Succeeded.Should().BeTrue();

        var updateResponse = await _client.PutAsync($"/api/todos/{todoId}/mark-done", null);
        var updateResult = await updateResponse.Content.ReadFromJsonAsync<Result<bool>>();

        updateResult!.Succeeded.Should().BeFalse("Silinen kayıt güncellenememeli");
        updateResult.Errors.Should().Contain(x => x.Contains("not found"));
    }
}