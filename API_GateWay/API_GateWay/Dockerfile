FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["API_GateWay/API_GateWay.csproj", "API_GateWay/"]
RUN dotnet restore "API_GateWay/API_GateWay.csproj"
COPY . .
WORKDIR "/src/API_GateWay"
RUN dotnet build "API_GateWay.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API_GateWay.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API_GateWay.dll"]