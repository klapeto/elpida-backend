FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Elpida.Web.Frontend.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /
COPY --from=publish /app/publish .
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80