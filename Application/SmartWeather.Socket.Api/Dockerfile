FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development

USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
COPY "./libs/SmartWeather.Entities" ./libs/SmartWeather.Entities
COPY "./libs/SmartWeather.Repositories" ./libs/SmartWeather.Repositories
COPY "./libs/SmartWeather.Services" ./libs/SmartWeather.Services
COPY "./Application/SmartWeather.Socket.Api" ./Application/SmartWeather.Socket.Api
RUN dotnet restore "./Application/SmartWeather.Socket.Api/SmartWeather.Socket.Api.csproj"
RUN dotnet build "./Application/SmartWeather.Socket.Api/SmartWeather.Socket.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Application/SmartWeather.Socket.Api/SmartWeather.Socket.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartWeather.Socket.Api.dll"]