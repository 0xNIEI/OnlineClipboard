#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8443
USER $APP_UID
ENV ASPNETCORE_URLS="https://+:8443;http://+:8080"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OnlineClipboard.csproj", "."]
RUN dotnet restore "./OnlineClipboard.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "OnlineClipboard.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineClipboard.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineClipboard.dll"]