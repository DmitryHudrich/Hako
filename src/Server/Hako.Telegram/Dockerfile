# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY Server/Hako.Telegram/*.sln .

COPY Protos/. ./proto/
COPY Server/Hako.Telegram/Telegram.Main/. ./Telegram.Main/
COPY Server/Hako.Telegram/Telegram.ServiceClient/. ./Telegram.ServiceClient/

WORKDIR /source/Telegram.Main
RUN dotnet publish -c release -o /app 

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Telegram.Main.dll"]