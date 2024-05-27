# Build stage for TCPListener
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-tcp
WORKDIR /App

COPY . .

ARG DATABASE_HOST
ARG DATABASE_NAME
ARG DATABASE_USER
ARG DATABASE_PASSWORD
ARG SERVER_ADDRESS

RUN echo "DATABASE_HOST=$DATABASE_HOST" >> /TCPListener/.env && \
    echo "DATABASE_NAME=$DATABASE_NAME" >> /TCPListener/.env && \
    echo "DATABASE_USER=$DATABASE_USER" >> /TCPListener/.env && \
    echo "DATABASE_PASSWORD=$DATABASE_PASSWORD" >> /TCPListener/.env && \
    echo "SERVER_ADDRESS=$SERVER_ADDRESS" >> /TCPListener/.env

RUN dotnet restore
RUN dotnet publish -c Release -o /App/TCPListener/out

# Build stage for WebAPI
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-web
WORKDIR /App
COPY . .

ARG DATABASE_HOST
ARG DATABASE_NAME
ARG DATABASE_USER
ARG DATABASE_PASSWORD
ARG SERVER_ADDRESS

RUN echo "DATABASE_HOST=$DATABASE_HOST" >> /WebAPI/.env && \
    echo "DATABASE_NAME=$DATABASE_NAME" >> /WebAPI/.env && \
    echo "DATABASE_USER=$DATABASE_USER" >> /WebAPI/.env && \
    echo "DATABASE_PASSWORD=$DATABASE_PASSWORD" >> /WebAPI/.env && \
    echo "SERVER_ADDRESS=$SERVER_ADDRESS" >> /WebAPI/.env

RUN dotnet restore
RUN dotnet publish -c Release -o /App/WebAPI/out

# Final stage for TCPListener
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final-tcp
WORKDIR /App
COPY --from=build-tcp /App/TCPListener/out .
EXPOSE 6868
ENTRYPOINT ["dotnet", "TCPListener.dll"]

# Final stage for WebAPI
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final-web
WORKDIR /App/WebAPI/out
COPY --from=build-web /App/WebAPI/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "WebAPI.dll"]
