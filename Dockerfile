# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project file
COPY ./AuthSolution/AuthSolution.sln .
COPY ./AuthSolution/Auth.API/Auth.API.csproj ./Auth.API/
RUN dotnet restore

# Copy the rest of the code
COPY ./AuthSolution/ ./AuthSolution/
WORKDIR /src/AuthSolution/Auth.API
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "Auth.API.dll"]




