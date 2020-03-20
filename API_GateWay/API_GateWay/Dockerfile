FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY API_GateWay/*.csproj API_GateWay/
RUN dotnet restore
COPY . .

# testing

# publish
FROM build AS publish
WORKDIR /src/API_GateWay
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS runtime
WORKDIR /app
COPY --from=publish /src/publish .

# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet API_GateWay.dll