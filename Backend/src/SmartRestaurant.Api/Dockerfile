FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/SmartRestaurant.Domain/SmartRestaurant.Domain.csproj src/SmartRestaurant.Domain/
COPY src/SmartRestaurant.Application/SmartRestaurant.Application.csproj src/SmartRestaurant.Application/
COPY src/SmartRestaurant.Infrastructure/SmartRestaurant.Infrastructure.csproj src/SmartRestaurant.Infrastructure/
COPY src/SmartRestaurant.Api/SmartRestaurant.Api.csproj src/SmartRestaurant.Api/
RUN dotnet restore src/SmartRestaurant.Api/SmartRestaurant.Api.csproj

COPY src/ src/
RUN dotnet publish src/SmartRestaurant.Api/SmartRestaurant.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SmartRestaurant.Api.dll"]
