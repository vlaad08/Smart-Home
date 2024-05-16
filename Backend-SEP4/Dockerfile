FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /App

COPY . ./

ARG DATABASE_HOST
ARG DATABASE_NAME
ARG DATABASE_USER
ARG DATABASE_PASSWORD

RUN echo "DATABASE_HOST=$DATABASE_HOST" >> .env && \
    echo "DATABASE_NAME=$DATABASE_NAME" >> .env && \
    echo "DATABASE_USER=$DATABASE_USER" >> .env && \
    echo "DATABASE_PASSWORD=$DATABASE_PASSWORD" >> .env

RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build-env /App/out .
COPY --from=build-env /App/.env .env
EXPOSE 80
ENTRYPOINT ["dotnet", "WebAPI.dll"]
