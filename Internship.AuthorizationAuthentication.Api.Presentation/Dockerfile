FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Internship.AuthorizationAuthentication.Api.Presentation/Internship.AuthorizationAuthentication.Api.Presentation.csproj", "Internship.AuthorizationAuthentication.Api.Presentation/"]
COPY ["Internship.AuthorizationAuthentication.Api.Core/Internship.AuthorizationAuthentication.Api.Core.csproj", "Internship.AuthorizationAuthentication.Api.Core/"]
COPY ["Internship.AuthorizationAuthentication.Api.Infrastructure/Internship.AuthorizationAuthentication.Api.Infrastructure.csproj", "Internship.AuthorizationAuthentication.Api.Infrastructure/"]
RUN dotnet restore "Internship.AuthorizationAuthentication.Api.Presentation/Internship.AuthorizationAuthentication.Api.Presentation.csproj"
COPY . .
WORKDIR "/src/Internship.AuthorizationAuthentication.Api.Presentation"
RUN dotnet build "Internship.AuthorizationAuthentication.Api.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Internship.AuthorizationAuthentication.Api.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Internship.AuthorizationAuthentication.Api.Presentation.dll"]
