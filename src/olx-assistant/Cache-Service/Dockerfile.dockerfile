FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build


COPY olx-assistant-cache-service/ /src/olx-assistant-cache-service/
COPY olx-assistant-domain/ /src/olx-assistant-domain/
COPY olx-assistant-contracts/ /src/olx-assistant-contracts/

RUN echo "Ahmetov Petuh: " && ls -la /src

WORKDIR src/olx-assistant-cache-service
RUN dotnet restore olx-assistant-cache-service.csproj

WORKDIR ../../

WORKDIR src/olx-assistant-cache-service
RUN dotnet build olx-assistant-cache-service.csproj -c Release --no-restore
RUN dotnet publish olx-assistant-cache-service.csproj -c Release -o /app/publish --no-build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

EXPOSE 5051

ENTRYPOINT ["dotnet", "olx-assistant-cache-service.dll"]
