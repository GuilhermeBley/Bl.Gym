# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Bl.Gym.TrainingApi.Api.csproj", "WebApp/"]
RUN dotnet restore "WebApp/Bl.Gym.TrainingApi.Api.csproj"
COPY . .
WORKDIR "/src/WebApp"
RUN dotnet build "Bl.Gym.TrainingApi.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Bl.Gym.TrainingApi.Api.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bl.Gym.TrainingApi.Api.dll"]