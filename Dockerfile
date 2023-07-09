#Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
ARG REGISTRY_USER
ARG REGISTRY_TOKEN
COPY . .
RUN dotnet nuget add source --username $REGISTRY_USER --password $REGISTRY_TOKEN --store-password-in-clear-text --name Deadalus "https://nuget.pkg.github.com/DaedalusTeam/index.json"
RUN dotnet restore "WebAPI/WebAPI.csproj"
RUN dotnet publish "WebAPI/WebAPI.csproj" -c Release -o /app --no-restore

#Serve stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY --from=build /app ./

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "WebAPI.dll"]