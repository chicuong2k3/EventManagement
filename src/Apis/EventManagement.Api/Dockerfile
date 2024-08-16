#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EventManagement/src/Apis/EventManagement.Api/EventManagement.Api.csproj", "EventManagement/src/Apis/EventManagement.Api/"]
COPY ["EventManagement/src/Common/EventManagement.Common.Api/EventManagement.Common.Api.csproj", "EventManagement/src/Common/EventManagement.Common.Api/"]
COPY ["EventManagement/src/Common/EventManagement.Common.Domain/EventManagement.Common.Domain.csproj", "EventManagement/src/Common/EventManagement.Common.Domain/"]
COPY ["EventManagement/src/Modules/EventModule/EventManagement.Events.Infrastructure/EventManagement.Events.Infrastructure.csproj", "EventManagement/src/Modules/EventModule/EventManagement.Events.Infrastructure/"]
COPY ["EventManagement/src/Common/EventManagement.Common.Infrastructure/EventManagement.Common.Infrastructure.csproj", "EventManagement/src/Common/EventManagement.Common.Infrastructure/"]
COPY ["EventManagement/src/Common/EventManagement.Common.Application/EventManagement.Common.Application.csproj", "EventManagement/src/Common/EventManagement.Common.Application/"]
COPY ["EventManagement/src/Modules/EventModule/EventManagement.Events.Application/EventManagement.Events.Application.csproj", "EventManagement/src/Modules/EventModule/EventManagement.Events.Application/"]
COPY ["EventManagement/src/Modules/EventModule/EventManagement.Events.Domain/EventManagement.Events.Domain.csproj", "EventManagement/src/Modules/EventModule/EventManagement.Events.Domain/"]
COPY ["EventManagement/src/Modules/EventModule/EventManagement.Events.PublicApi/EventManagement.Events.PublicApi.csproj", "EventManagement/src/Modules/EventModule/EventManagement.Events.PublicApi/"]
COPY ["EventManagement/src/Modules/TicketingModule/EventManagement.Ticketing.Infrastructure/EventManagement.Ticketing.Infrastructure.csproj", "EventManagement/src/Modules/TicketingModule/EventManagement.Ticketing.Infrastructure/"]
COPY ["EventManagement/src/Modules/TicketingModule/EventManagement.Ticketing.Application/EventManagement.Ticketing.Application.csproj", "EventManagement/src/Modules/TicketingModule/EventManagement.Ticketing.Application/"]
COPY ["EventManagement/src/Modules/UserModule/EventManagement.Users.IntegrationEvents/EventManagement.Users.IntegrationEvents.csproj", "EventManagement/src/Modules/UserModule/EventManagement.Users.IntegrationEvents/"]
COPY ["EventManagement/src/Modules/TicketingModule/EventManagement.Ticketing.Domain/EventManagement.Ticketing.Domain.csproj", "EventManagement/src/Modules/TicketingModule/EventManagement.Ticketing.Domain/"]
COPY ["EventManagement/src/Modules/UserModule/EventManagement.Users.Infrastructure/EventManagement.Users.Infrastructure.csproj", "EventManagement/src/Modules/UserModule/EventManagement.Users.Infrastructure/"]
COPY ["EventManagement/src/Modules/UserModule/EventManagement.Users.Application/EventManagement.Users.Application.csproj", "EventManagement/src/Modules/UserModule/EventManagement.Users.Application/"]
COPY ["EventManagement/src/Modules/UserModule/EventManagement.Users.Domain/EventManagement.Users.Domain.csproj", "EventManagement/src/Modules/UserModule/EventManagement.Users.Domain/"]
RUN dotnet restore "./EventManagement/src/Apis/EventManagement.Api/EventManagement.Api.csproj"
COPY . .
WORKDIR "/src/EventManagement/src/Apis/EventManagement.Api"
RUN dotnet build "./EventManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EventManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventManagement.Api.dll"]