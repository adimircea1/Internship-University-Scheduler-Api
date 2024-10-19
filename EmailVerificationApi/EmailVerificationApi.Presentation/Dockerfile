FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EmailVerificationApi.Presentation/EmailVerificationApi.Presentation.csproj", "EmailVerificationApi.Presentation/"]
COPY ["EmailVerificationApi.Core/EmailVerificationApi.Core.csproj", "EmailVerificationApi.Core/"]
COPY ["EmailVerification.Library/EmailVerification.Library.csproj", "EmailVerification.Library/"]
RUN dotnet restore "EmailVerificationApi.Presentation/EmailVerificationApi.Presentation.csproj"
COPY . .
WORKDIR "/src/EmailVerificationApi.Presentation"
RUN dotnet build "EmailVerificationApi.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmailVerificationApi.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailVerificationApi.Presentation.dll"]
