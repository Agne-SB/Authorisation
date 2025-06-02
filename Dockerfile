# Use the .NET SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY *.sln .
COPY Authorisation/*.csproj ./Authorisation/
RUN dotnet restore

# Copy the rest of the files and build
COPY . .
WORKDIR /src/Authorisation
RUN dotnet publish -c Release -o /app/publish

# Use a lightweight runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (optional for local test)
EXPOSE 80

ENTRYPOINT ["dotnet", "Authorisation.dll"]
