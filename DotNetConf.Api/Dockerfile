#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["dotnet-conf-2021.Api/dotnet-conf-2021.Api.csproj", "dotnet-conf-2021.Api/"]
RUN dotnet restore "dotnet-conf-2021.Api/dotnet-conf-2021.Api.csproj"
COPY . .
WORKDIR "/src/dotnet-conf-2021.Api"
RUN dotnet build "dotnet-conf-2021.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dotnet-conf-2021.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dotnet-conf-2021.Api.dll"]