FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SmartRestaurant.Api.csproj .
RUN dotnet restore SmartRestaurant.Api.csproj

COPY . .
RUN dotnet publish SmartRestaurant.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SmartRestaurant.Api.dll"]
