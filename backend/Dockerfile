FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY EventRep.sln ./
COPY src/EventRep.Domain/EventRep.Domain.csproj src/EventRep.Domain/
COPY src/EventRep.Application/EventRep.Application.csproj src/EventRep.Application/
COPY src/EventRep.Infrastructure/EventRep.Infrastructure.csproj src/EventRep.Infrastructure/
COPY src/EventRep.Api/EventRep.Api.csproj src/EventRep.Api/
RUN dotnet restore src/EventRep.Api/EventRep.Api.csproj
COPY . .
RUN dotnet publish src/EventRep.Api/EventRep.Api.csproj -c Release --no-restore -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "EventRep.Api.dll"]
