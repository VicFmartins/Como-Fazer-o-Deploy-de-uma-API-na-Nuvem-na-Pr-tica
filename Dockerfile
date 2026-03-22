FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/MinhaApi/MinhaApi.csproj", "src/MinhaApi/"]
RUN dotnet restore "src/MinhaApi/MinhaApi.csproj"

COPY . .
RUN dotnet publish "src/MinhaApi/MinhaApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MinhaApi.dll"]
