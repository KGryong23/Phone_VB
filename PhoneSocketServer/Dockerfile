FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PhoneSocketServer/PhoneSocketServer.csproj", "PhoneSocketServer/"]
RUN dotnet restore "PhoneSocketServer/PhoneSocketServer.csproj"
COPY . .
WORKDIR "/src/PhoneSocketServer"
RUN dotnet build "PhoneSocketServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PhoneSocketServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhoneSocketServer.dll"]
