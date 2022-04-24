#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UREC-api.csproj", "."]
RUN dotnet restore "./UREC-api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "UREC-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UREC-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UREC-api.dll"]
# ENTRYPOINT [ "dotnet", "HerokuApp.dll" ]
# Use the following instead for Heroku
CMD ASPNETCORE_URLS=http://*:$PORT dotnet UREC-api.dll