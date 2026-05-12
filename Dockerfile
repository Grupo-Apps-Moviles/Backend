# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar el archivo de solución y el proyecto
COPY ["Backend-Frock.sln", "./"]
COPY ["Backend-Frock/Backend-Frock.csproj", "Backend-Frock/"]

# Restaurar dependencias
RUN dotnet restore "Backend-Frock/Backend-Frock.csproj"

# Copiar el resto del código y compilar
COPY . .
WORKDIR "/src/Backend-Frock"
RUN dotnet build "Backend-Frock.csproj" -c Release -o /app/build

# Publicar el backend
FROM build AS publish
RUN dotnet publish "Backend-Frock.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final para correr la app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Exponer el puerto que Railway asignará
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Backend-Frock.dll"]