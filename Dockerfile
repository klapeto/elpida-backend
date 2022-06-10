FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app 

# copy everything and build the project
COPY . ./
RUN dotnet restore src/Elpida.Backend/*.csproj
RUN dotnet publish src/Elpida.Backend/*.csproj -c Release -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "Elpida.Backend.dll"]