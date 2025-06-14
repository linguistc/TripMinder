# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy only needed .csproj files
COPY *.sln .
COPY TripMinder.API/*.csproj TripMinder.API/
COPY TripMinder.Core/*.csproj TripMinder.Core/
COPY TripMinder.Data/*.csproj TripMinder.Data/
COPY TripMinder.Infrastructure/*.csproj TripMinder.Infrastructure/
COPY TripMinder.Service/*.csproj TripMinder.Service/

# Restore without the test project
RUN dotnet sln remove TripMinder.Core.Tests/TripMinder.Core.Tests.csproj || true
RUN dotnet restore

# Copy the rest of the code
COPY . .

# Publish only API
RUN dotnet publish TripMinder.API/TripMinder.API.csproj -c Release -o /app/publish

# Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TripMinder.API.dll"]