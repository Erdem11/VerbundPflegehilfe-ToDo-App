FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["VerbundPflegehilfe.API/VerbundPflegehilfe.API.csproj", "VerbundPflegehilfe.API/"]
RUN dotnet restore "VerbundPflegehilfe.API/VerbundPflegehilfe.API.csproj"
COPY . .
WORKDIR "/src/VerbundPflegehilfe.API"
RUN dotnet build "./VerbundPflegehilfe.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VerbundPflegehilfe.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VerbundPflegehilfe.API.dll"]
