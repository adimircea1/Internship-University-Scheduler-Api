FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Internship.UniversityScheduler.Api.Presentation/Internship.UniversityScheduler.Api.Presentation.csproj", "Internship.UniversityScheduler.Api.Presentation/"]
COPY ["Internship.UniversityScheduler.Api.Core/Internship.UniversityScheduler.Api.Core.csproj", "Internship.UniversityScheduler.Api.Core/"]
COPY ["Internship.UniversityScheduler.Library/Internship.UniversityScheduler.Library.csproj", "Internship.UniversityScheduler.Library/"]
COPY ["Internship.UniversityScheduler.Api.Infrastructure/Internship.UniversityScheduler.Api.Infrastructure.csproj", "Internship.UniversityScheduler.Api.Infrastructure/"]
RUN dotnet restore "Internship.UniversityScheduler.Api.Presentation/Internship.UniversityScheduler.Api.Presentation.csproj"
COPY . .
WORKDIR "/src/Internship.UniversityScheduler.Api.Presentation"
RUN dotnet build "Internship.UniversityScheduler.Api.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Internship.UniversityScheduler.Api.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Internship.UniversityScheduler.Api.Presentation.dll"]
