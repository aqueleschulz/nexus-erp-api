#Buildar
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

#Copiar primeiro arquivo e restaurar dependencias
COPY ["NexusERP.API.csproj", "./"]
RUN dotnet restore "NexusERP.API.csproj"

#Copiar o restante dos arquivos e compilar
COPY . .
RUN dotnet publish "NexusERP.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

#Usar imagem runtime, apenas com o necessario para rodar a aplicacao
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

#Copiar os arquivos publicados da etapa de build
COPY --from=build /app/publish .

#Definir o ponto de entrada da aplicacao
ENTRYPOINT ["dotnet", "NexusERP.API.dll"]