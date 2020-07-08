FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app 

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Elpida.Backend/*.csproj ./Elpida.Backend/
COPY Elpida.Backend.Data/*.csproj ./Elpida.Backend.Data/
COPY Elpida.Backend.Data.Abstractions/*.csproj ./Elpida.Backend.Data.Abstractions/
COPY Elpida.Backend.Services/*.csproj ./Elpida.Backend.Services/
COPY Elpida.Backend.Services.Abstractions/*.csproj ./Elpida.Backend.Services.Abstractions/
#
RUN dotnet restore 

# copy everything else and build app
COPY Elpida.Backend/. ./Elpida.Backend/
COPY Elpida.Backend.Data/. ./Elpida.Backend.Data/
COPY Elpida.Backend.Data.Abstractions/. ./Elpida.Backend.Data.Abstractions/
COPY Elpida.Backend.Services/. ./Elpida.Backend.Services/
COPY Elpida.Backend.Services.Abstractions/. ./Elpida.Backend.Services.Abstractions/

WORKDIR /app/Elpida.Backend
RUN dotnet publish -c Release -o out 

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app 
#
COPY --from=build /app/Elpida.Backend/out ./
ENTRYPOINT ["dotnet", "Elpida.Backend.dll"]