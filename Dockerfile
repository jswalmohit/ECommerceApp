#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ECommerceApp.sln", "."]
COPY ["src/ECommerceApp/ECommerceApp.csproj", "src/ECommerceApp/"]
COPY ["src/EComm.Commons/EComm.Commons.csproj", "src/EComm.Commons/"]
COPY ["src/EComm.Data/EComm.Data.csproj", "src/EComm.Data/"]
COPY ["src/EComm.Repositories/EComm.Repositories.csproj", "src/EComm.Repositories/"]
COPY ["src/EComm.Services/EComm.Services.csproj", "src/EComm.Services/"]
COPY ["src/EComm.Logging/EComm.Logging.csproj", "src/EComm.Logging/"]
RUN dotnet restore "ECommerceApp.sln"
COPY . .
WORKDIR "/src/src/ECommerceApp"
RUN dotnet build "ECommerceApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ECommerceApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerceApp.dll"]