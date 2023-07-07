#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS publish
WORKDIR /src
COPY . .

ARG REGISTRY_USER
ARG REGISTRY_TOKEN

RUN dotnet nuget add source --username  $REGISTRY_USER --password $REGISTRY_TOKEN --store-password-in-clear-text --name Deadalus "https://nuget.pkg.github.com/DaedalusTeam/index.json"
RUN dotnet restore
WORKDIR "/src/WebAPI"
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]