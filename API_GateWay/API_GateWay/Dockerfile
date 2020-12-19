FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim  AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . ./aspnetapp/
WORKDIR /app/aspnetapp
RUN dotnet restore

# Copy everything else and build
COPY . ./API_GateWay/
RUN dotnet publish "API_GateWay" -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as final
WORKDIR /app
COPY --from=build-env /app .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet API_GateWay.dll --environment "Production"