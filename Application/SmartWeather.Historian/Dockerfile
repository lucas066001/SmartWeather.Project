#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
COPY "./libs/SmartWeather.Entities" ./libs/SmartWeather.Entities
COPY "./libs/SmartWeather.Repositories" ./libs/SmartWeather.Repositories
COPY "./libs/SmartWeather.Services" ./libs/SmartWeather.Services
COPY "./Application/SmartWeather.Historian" ./Application/SmartWeather.Historian
RUN dotnet restore "./Application/SmartWeather.Historian/SmartWeather.Historian.csproj"
RUN dotnet build "./Application/SmartWeather.Historian/SmartWeather.Historian.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Application/SmartWeather.Historian/SmartWeather.Historian.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartWeather.Historian.dll"]