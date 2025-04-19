# المرحلة الأولى: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY *.sln .
COPY TripMinder.API/*.csproj TripMinder.API/
COPY TripMinder.Core/*.csproj TripMinder.Core/
COPY TripMinder.Data/*.csproj TripMinder.Data/
COPY TripMinder.Infrastructure/*.csproj TripMinder.Infrastructure/
COPY TripMinder.Service/*.csproj TripMinder.Service/

RUN dotnet restore

COPY . .

RUN dotnet publish TripMinder.API/TripMinder.API.csproj -c Release -o /app/publish

# المرحلة الثانية: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TripMinder.API.dll"]