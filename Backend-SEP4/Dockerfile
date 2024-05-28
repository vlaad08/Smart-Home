# Build stage for TCPListener
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-tcp
WORKDIR /App

COPY . .

ARG DATABASE_HOST
ARG DATABASE_NAME
ARG DATABASE_USER
ARG DATABASE_PASSWORD
ARG SERVER_ADDRESS

RUN echo "DATABASE_HOST=$DATABASE_HOST" >> .env && \
    echo "DATABASE_NAME=$DATABASE_NAME" >> .env && \
    echo "DATABASE_USER=$DATABASE_USER" >> .env && \
    echo "DATABASE_PASSWORD=$DATABASE_PASSWORD" >> .env && \
    echo "SERVER_ADDRESS=$SERVER_ADDRESS" >> .env

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

RUN echo "DATABASE_HOST=$DATABASE_HOST" >> .env && \
    echo "DATABASE_NAME=$DATABASE_NAME" >> .env && \
    echo "DATABASE_USER=$DATABASE_USER" >> .env && \
    echo "DATABASE_PASSWORD=$DATABASE_PASSWORD" >> .env && \
    echo "SERVER_ADDRESS=$SERVER_ADDRESS" >> .env

RUN dotnet restore
RUN dotnet publish -c Release -o /App/WebAPI/out

# Final stage for TCPListener
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final-tcp
COPY --from=build-tcp /App/TCPListener/out .
COPY --from=build-tcp /App/.env .env
EXPOSE 6868
ENTRYPOINT ["dotnet", "TCPListener.dll"]


# Final stage for WebAPI
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final-web
WORKDIR /App/WebAPI/out
COPY --from=build-web /App/WebAPI/out .
COPY --from=build-web /App/.env .env
EXPOSE 8080
ENTRYPOINT ["dotnet", "WebAPI.dll"]
