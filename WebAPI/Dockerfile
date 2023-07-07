#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

ARG REGISTRY_USER
ARG REGISTRY_TOKEN

COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["Logic/Logic.csproj", "Logic/"]
RUN dotnet nuget add source --username  $REGISTRY_USER --password $REGISTRY_TOKEN --store-password-in-clear-text --name Deadalus "https://nuget.pkg.github.com/DaedalusTeam/index.json"
RUN dotnet restore "WebAPI/WebAPI.csproj"
COPY . .
WORKDIR "/src/WebAPI"
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]