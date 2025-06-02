FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./AuthSolution/AuthSolution.sln .
COPY ./AuthSolution/Auth.API/Auth.API.csproj ./Auth.API/
COPY ./AuthSolution/Auth.Domain/Auth.Domain.csproj ./Auth.Domain/
COPY ./AuthSolution/Auth.Application/Auth.Application.csproj ./Auth.Application/
COPY ./AuthSolution/Auth.Infrastructure/Auth.Infrastructure.csproj ./Auth.Infrastructure/
COPY ./AuthSolution/PasswordTest/PasswordTest.csproj ./PasswordTest/

RUN dotnet restore AuthSolution.sln

COPY ./AuthSolution/ ./AuthSolution/
WORKDIR /src/AuthSolution/Auth.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "Auth.API.dll"]
