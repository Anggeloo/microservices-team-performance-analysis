# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /servicerawmaterialmanagement

EXPOSE 90
EXPOSE 4000

COPY ./*.csproj ./
RUN dotnet restore 

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:8.0 
WORKDIR /servicerawmaterialmanagement
COPY --from=build /servicerawmaterialmanagement/out .
ENTRYPOINT ["dotnet", "microservices-raw-material-management.dll"]
