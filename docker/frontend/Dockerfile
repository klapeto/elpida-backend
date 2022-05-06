FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app 

# copy everything and build the project
COPY . ./
RUN apt-get update
RUN apt-get install -y python3
RUN dotnet workload install wasm-tools
RUN dotnet restore src/Elpida.Web.Frontend/*.csproj
RUN dotnet publish src/Elpida.Web.Frontend/*.csproj -c Release -o /app/publish

# build runtime image
FROM nginx:alpine AS final
WORKDIR /
COPY --from=build /app/publish .
COPY /src/Elpida.Web.Frontend/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80