FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Elpida.Web.Frontend/Elpida.Web.Frontend.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build Elpida.Web.Frontend/Elpida.Web.Frontend.csproj

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish Elpida.Web.Frontend/Elpida.Web.Frontend.csproj

FROM nginx:alpine AS final
WORKDIR /
COPY --from=publish /app/publish .
COPY Elpida.Web.Frontend/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80