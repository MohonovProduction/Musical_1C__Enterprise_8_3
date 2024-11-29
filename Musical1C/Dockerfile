FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Musical1C/Musical1C.csproj", "Musical1C/"]
COPY ["ConsoleView/ConsoleView.csproj", "ConsoleView/"]
COPY ["Presenter/Presenter.csproj", "Presenter/"]
COPY ["Storage/Storage.csproj", "Storage/"]
RUN dotnet restore "Musical1C/Musical1C.csproj"
COPY . .
WORKDIR "/src/Musical1C"
RUN dotnet build "Musical1C.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Musical1C.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Musical1C.dll"]
