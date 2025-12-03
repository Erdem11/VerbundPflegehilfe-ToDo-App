# Backend – VerbundPflegehilfe

This folder contains the ASP.NET Core Web API for the VerbundPflegehilfe task.

It exposes simple TODO endpoints that are used by the frontend.

## Structure

- `src/VerbundPflegehilfe.API/` – Web API project
- `src/VerbundPflegehilfe.Application/` – Application layer (use cases, logic)
- `src/VerbundPflegehilfe.Domain/` – Domain layer (entities, events)
- `src/VerbundPflegehilfe.Infrastructure/` – Infrastructure (database, persistence)
- `tests/VerbundPflegehilfe.UnitTests/` – Unit tests
- `tests/VerbundPflegehilfe.IntegrationTests/` – Integration tests

## Requirements

- .NET SDK (version from `global.json`, for example .NET 8)
- SQL Server (for development, can use Docker)
- appsettings.json configuration for **database connection** and **CORS**

## Run the API

1. Open a terminal in the `backend` folder.
2. Restore packages:
   ```powershell
   dotnet restore
   ```
3. Run the Web API project:
   ```powershell
   dotnet run --project src/VerbundPflegehilfe.API/VerbundPflegehilfe.API.csproj
   ```
4. Run the tests (optional):

    ```powershell
    dotnet test
    ```