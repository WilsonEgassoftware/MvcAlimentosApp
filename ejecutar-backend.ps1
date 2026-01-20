# Script para ejecutar el Backend
# Ejecuta este archivo con: .\ejecutar-backend.ps1

Write-Host "🚀 Iniciando Backend..." -ForegroundColor Green

# Navegar al directorio Backend
$backendPath = Join-Path $PSScriptRoot "Backend"

if (-not (Test-Path $backendPath)) {
    Write-Host "❌ Error: No se encuentra la carpeta Backend" -ForegroundColor Red
    Write-Host "   Ruta esperada: $backendPath" -ForegroundColor Yellow
    exit 1
}

Set-Location $backendPath

Write-Host "📂 Directorio: $backendPath" -ForegroundColor Cyan

# Verificar que existe el proyecto
if (-not (Test-Path "SupermarketAPI.csproj")) {
    Write-Host "❌ Error: No se encuentra SupermarketAPI.csproj" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Proyecto encontrado" -ForegroundColor Green
Write-Host ""
Write-Host "🔧 Ejecutando dotnet run..." -ForegroundColor Yellow
Write-Host ""

# Ejecutar el proyecto
dotnet run
