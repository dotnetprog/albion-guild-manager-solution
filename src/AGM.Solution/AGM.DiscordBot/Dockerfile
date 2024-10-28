#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AGM.DiscordBot/AGM.DiscordBot.csproj", "AGM.DiscordBot/"]
COPY ["AGM.Application/AGM.Application.csproj", "AGM.Application/"]
COPY ["AGM.Domain/AGM.Domain.csproj", "AGM.Domain/"]
COPY ["AGM.Database/AGM.Database.csproj", "AGM.Database/"]
COPY ["AGM.EntityFramework/AGM.EntityFramework.csproj", "AGM.EntityFramework/"]
RUN dotnet restore "./AGM.DiscordBot/./AGM.DiscordBot.csproj"
COPY . .
WORKDIR "/src/AGM.DiscordBot"
RUN dotnet build "./AGM.DiscordBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AGM.DiscordBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AGM.DiscordBot.dll"]