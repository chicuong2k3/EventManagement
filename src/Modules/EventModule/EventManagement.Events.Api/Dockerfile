#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Modules/EventModule/EventManagement.Events.Api/EventManagement.Events.Api.csproj", "src/Modules/EventModule/EventManagement.Events.Api/"]
COPY ["src/Common/EventManagement.Common.Domain/EventManagement.Common.Domain.csproj", "src/Common/EventManagement.Common.Domain/"]
COPY ["src/Modules/EventModule/EventManagement.Events.Application/EventManagement.Events.Application.csproj", "src/Modules/EventModule/EventManagement.Events.Application/"]
COPY ["src/Common/EventManagement.Common.Application/EventManagement.Common.Application.csproj", "src/Common/EventManagement.Common.Application/"]
COPY ["src/Modules/EventModule/EventManagement.Events.Domain/EventManagement.Events.Domain.csproj", "src/Modules/EventModule/EventManagement.Events.Domain/"]
COPY ["src/Modules/EventModule/EventManagement.Events.Infrastructure/EventManagement.Events.Infrastructure.csproj", "src/Modules/EventModule/EventManagement.Events.Infrastructure/"]
COPY ["src/Common/EventManagement.Common.Infrastructure/EventManagement.Common.Infrastructure.csproj", "src/Common/EventManagement.Common.Infrastructure/"]
RUN dotnet restore "./src/Modules/EventModule/EventManagement.Events.Api/EventManagement.Events.Api.csproj"
COPY . .
WORKDIR "/src/src/Modules/EventModule/EventManagement.Events.Api"
RUN dotnet build "./EventManagement.Events.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EventManagement.Events.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventManagement.Events.Api.dll"]