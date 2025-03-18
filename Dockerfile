FROM node:18 AS node_build

WORKDIR /app

COPY /src/vue/package.json ./


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

