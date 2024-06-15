# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.sln .

COPY Telegram.Main/*.csproj ./Telegram.Main/
COPY Telegram.ServiceClient/*.csproj ./Telegram.ServiceClient/

RUN dotnet restore

# copy everything else and build app
COPY Telegram.Main/. ./Telegram.Main/
COPY Telegram.ServiceClient/. ./Telegram.ServiceClient/

WORKDIR /source/Telegram.Main
RUN dotnet publish -c release -o /app --no-restore 

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Telegram.Main.dll"]