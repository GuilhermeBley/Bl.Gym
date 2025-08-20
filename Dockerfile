FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY ["Bl.Gym.sln", "./"]
COPY ["src/", "src/"]

# 3. Restore dependencies for all projects
RUN dotnet restore "src/training-api/Bl.Gym.TrainingApi.Api/Bl.Gym.TrainingApi.Api.csproj"

# 5. Build all projects
RUN dotnet build "src/training-api/Bl.Gym.TrainingApi.Api/Bl.Gym.TrainingApi.Api.csproj" -c Release --no-restore -o /app/build

# 6. Publish only the main API project (it will include dependencies)
FROM build AS publish
RUN dotnet publish "src/training-api/Bl.Gym.TrainingApi.Api/Bl.Gym.TrainingApi.Api.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
EXPOSE 8081
#ENV ASPNETCORE_URLS=http://+:8080
#ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "Bl.Gym.TrainingApi.Api.dll"]