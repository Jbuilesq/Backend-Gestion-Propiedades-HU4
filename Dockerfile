# 1. Definir imagen base (Etapa Build)   ---   ---   ----   ---   ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# 2. Definir directorio donde copiamos proyecto
WORKDIR /src

# 3. Copiar archivos .csproj
COPY ["property.Api/property.Api.csproj", "property.Api/"]
COPY ["property.Application/property.Application.csproj", "property.Application/"]
COPY ["property.Domain/property.Domain.csproj", "property.Domain/"]
COPY ["property.Infrastructure/property.Infrastructure.csproj", "property.Infrastructure/"]

# 4. Restaurar dependencias
RUN dotnet restore "property.Api/property.Api.csproj"

# 5. Copiamos todo el codigo fuente
COPY . .

# 6. Compilar el proyecto y guardar
RUN dotnet build "property.Api/property.Api.csproj" -c Release -o /app/build

# 7. Publicacion (Etapa Publish)   ---   ---   ---   ---
FROM build AS publish

# 8. Definir directorio donde vamos a publicar
WORKDIR /src

# 9. Compilar y guardar para publicar
RUN dotnet publish "property.Api/property.Api.csproj" -c Release -o /app/publish

# 10. Imagen final (Etapa Final) --   ---   ---   ---   ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# 11. Directorio de trabajo
WORKDIR /app

# 12. Copiar proyecto de estapa publicacion
COPY --from=publish /app/publish .

# 13. Exponer el puerto
EXPOSE 8080

# 14. EntryPoint
ENTRYPOINT ["dotnet", "property.Api.dll"]

