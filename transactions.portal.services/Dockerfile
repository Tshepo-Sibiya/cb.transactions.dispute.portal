FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["transactions.portal.services.csproj", "./"]
RUN dotnet restore "./transactions.portal.services.csproj"
COPY . .
RUN dotnet build "./transactions.portal.services.csproj" -c Release -o /app/build
RUN dotnet publish "./transactions.portal.services.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "transactions.portal.services.dll"]

