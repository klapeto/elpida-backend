FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Elpida.Web.Frontend/Elpida.Web.Frontend.csproj", "Elpida.Web.Frontend/"]
RUN dotnet restore "Elpida.Web.Frontend/Elpida.Web.Frontend.csproj"
COPY . .
WORKDIR "/src/Elpida.Web.Frontend"
RUN dotnet build "Elpida.Web.Frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Elpida.Web.Frontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Elpida.Web.Frontend.dll"]
