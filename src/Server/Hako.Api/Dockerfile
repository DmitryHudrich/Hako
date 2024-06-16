# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY Server/Hako.Api/*.sln .

COPY Protos/. ./proto/
COPY Server/Hako.Api/Api.Public/. ./Api.Public/
COPY Server/Hako.Api/Api.ServiceClient/. ./Api.ServiceClient/

WORKDIR /source/Api.Public/
RUN dotnet publish -c release -o /app 

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Api.Public.dll"]