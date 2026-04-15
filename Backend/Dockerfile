FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file first for better layer caching
COPY Backend.csproj ./
RUN dotnet restore ./Backend.csproj

# Copy source and publish in Release mode
COPY . .
RUN dotnet publish ./Backend.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_EnableDiagnostics=0

EXPOSE 8080

# Run as non-root (provided by official .NET images)
USER $APP_UID

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]
