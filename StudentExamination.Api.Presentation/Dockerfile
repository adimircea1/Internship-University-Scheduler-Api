FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["StudentExamination.Api.Presentation/StudentExamination.Api.Presentation.csproj", "StudentExamination.Api.Presentation/"]
COPY ["StudentExamination.Api.Core/StudentExamination.Api.Core.csproj", "StudentExamination.Api.Core/"]
COPY ["StudentExamination.Api.Infrastructure/StudentExamination.Api.Infrastructure.csproj", "StudentExamination.Api.Infrastructure/"]
RUN dotnet restore "StudentExamination.Api.Presentation/StudentExamination.Api.Presentation.csproj"
COPY . .
WORKDIR "/src/StudentExamination.Api.Presentation"
RUN dotnet build "StudentExamination.Api.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StudentExamination.Api.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentExamination.Api.Presentation.dll"]
