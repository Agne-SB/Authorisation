# Use .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files using correct names
COPY ./AuthSolution.sln .
COPY ./Authorisation/Authorisation.csproj ./Authorisation/
RUN dotnet restore

# Copy everything else
COPY . .
WORKDIR /src/Authorisation
RUN dotnet publish -c Release -o /app/publish

# Use ASP.NET Core runtime image for final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "Authorisation.dll"]


