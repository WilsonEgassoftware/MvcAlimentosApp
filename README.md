
# MvcAlimentosApp
Aplicación ASP.NET Core MVC para gestión de alimentos (CRUD) con Login y roles (Admin/Usuario).

## Requisitos
- .NET 8.0 (o .NET 6.0)
- Visual Studio 2022/2023 o `dotnet` CLI
- (Opcional) Docker para despliegue en Render
# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish MvcAlimentosApp/MvcAlimentosApp.csproj -c Release -o /app/publish
-------------------------------------------------------------------------
# docker
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "MvcAlimentosApp.dll"]
------------------------------------------------------------
Azure AZZP
az login
az group create -n rg-mvc-alimentos -l eastus
az appservice plan create -n plan-mvc -g rg-mvc-alimentos --sku B1
az webapp create -n mvc-alimentos-<TU_SUFFIX> -g rg-mvc-alimentos -p plan-mvc --runtime "DOTNETCORE|8.0"
