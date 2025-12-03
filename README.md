# VerbundPflegehilfe Task

This is a small demo project with a **backend** (ASP.NET Core Web API) and a **frontend** (React + Vite).

## Project structure

- `backend/` – ASP.NET Core Web API with TODO example
- `frontend/` – React + TypeScript app that calls the API
- `docker-compose.yaml` – Optional: run backend and frontend with Docker

## Requirements

- .NET SDK (version defined in `backend/global.json`, for example .NET 8)
- Node.js (LTS) and npm
- Docker Desktop (only if you want to use Docker)

## Run the backend

1. Open a terminal in the `backend` folder.
2. Restore and run the API:

   ```powershell
   dotnet restore
   dotnet run --project src/VerbundPflegehilfe.API/VerbundPflegehilfe.API.csproj
   ```
