#
# if you want to run this manually, run this from Hako/src/ with
#
# docker build -t hako-server-dockerimage -f ./Server/Hako.Server/Dockerfile .  
#  

# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY Protos/. ./proto/
COPY Server/Hako.Server/Server.Domain/. ./Server.Domain/
COPY Server/Hako.Server/Server.ServerGate/. ./Server.ServerGate/

WORKDIR /source/Server.ServerGate
RUN dotnet publish -c release -o /app 

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Server.ServerGate.dll"]

# todo: control panel is not implemented in docker yet