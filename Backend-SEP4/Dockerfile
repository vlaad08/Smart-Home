FROM mcr.microsoft.com/dotnet/sdk:.07 AS build-env
WORKDIR /App

COPY . ./

RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "TCPListener.dll", "Intermediary-DB.dll", "WebAPI.dll", "TCPListenerDemo.dll"]