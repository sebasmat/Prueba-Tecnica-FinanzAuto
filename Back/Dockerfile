# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copiar archivos de proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo lo demás y publicar
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa final: Imagen de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .

# Exponer el puerto de la API
EXPOSE 80
ENTRYPOINT ["dotnet", "Back.dll"]