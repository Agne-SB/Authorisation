FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and csproj file correctly
COPY Authorisation.sln .
COPY Authorisation/Authorisation.csproj ./Authorisation/
RUN dotnet restore

# Copy the rest of the app
COPY . .
WORKDIR /src/Authorisation
RUN dotnet publish -c Release -o /app/publish

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "Authorisation.dll"]

