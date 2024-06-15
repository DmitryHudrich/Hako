# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.sln .

# COPY Server.Domain/*.csproj ./Server.Domain/
# COPY Server.ControlPanel/*.csproj ./Server.ControlPanel/
# COPY Server.Logic/*.csproj ./Server.Logic/
# COPY Server.ServerGate/*.csproj ./Server.ServerGate/
# COPY Server.Persistence/*.csproj ./Server.Persistence/
# COPY Server.Utils/*.csproj ./Server.Utils/

# RUN dotnet restore

# copy everything else and build app
COPY Server.Domain/. ./Server.Domain/
COPY Server.ControlPanel/. ./Server.ControlPanel/
COPY Server.Logic/. ./Server.Logic/
COPY Server.ServerGate/. ./Server.ServerGate/
COPY Server.Persistence/. ./Server.Persistence/
COPY Server.Utils/. ./Server.Utils/

WORKDIR /source/Server.ServerGate
RUN dotnet publish -c release -o /app 

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Server.ServerGate.dll"]

# todo: control panel is not implemented in docker yet