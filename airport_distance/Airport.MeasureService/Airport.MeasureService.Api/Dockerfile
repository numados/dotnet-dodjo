FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Airport.MeasureService.Api/Airport.MeasureService.Api.csproj", "Airport.MeasureService.Api/"]
RUN dotnet restore "Airport.MeasureService.Api/Airport.MeasureService.Api.csproj"
COPY . .
WORKDIR "/src/Airport.MeasureService.Api"
RUN dotnet build "Airport.MeasureService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Airport.MeasureService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Airport.MeasureService.Api.dll"]
