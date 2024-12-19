FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development

USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /Application
COPY "./Application/SmartWeather.Entities" ./SmartWeather.Entities
COPY "./Application/SmartWeather.Repositories" ./SmartWeather.Repositories
COPY "./Application/SmartWeather.Services" ./SmartWeather.Services
WORKDIR /src/Acquisition
COPY "./Acquisition/SmartWeather.StationMocker" .
RUN dotnet restore "./SmartWeather.StationMocker.csproj"
RUN dotnet build "./SmartWeather.StationMocker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SmartWeather.StationMocker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartWeather.StationMocker.dll"]