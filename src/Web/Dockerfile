FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY signalr-scale-out.sln ./
COPY docker-compose.dcproj ./
COPY Web/Web.csproj Web/

RUN dotnet restore signalr-scale-out.sln
COPY . .
WORKDIR /src/Web
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Web.dll"]
