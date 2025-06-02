FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and all project files
COPY ./AuthSolution/AuthSolution.sln .
COPY ./AuthSolution/Auth.API/Auth.API.csproj ./Auth.API/
COPY ./AuthSolution/Auth.Domain/Auth.Domain.csproj ./Auth.Domain/
COPY ./AuthSolution/Auth.Application/Auth.Application.csproj ./Auth.Application/
COPY ./AuthSolution/Auth.Infrastructure/Auth.Infrastructure.csproj ./Auth.Infrastructure/
COPY ./AuthSolution/PasswordTest/PasswordTest.csproj ./PasswordTest/

# Restore solution (handles all dependencies)
RUN dotnet restore AuthSolution.sln

# Copy the full source tree
COPY ./AuthSolution/ ./AuthSolution/

# Build only the API project for deployment
WORKDIR /src/AuthSolution/Auth.API
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "Auth.API.dll"]
