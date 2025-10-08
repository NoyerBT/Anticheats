# Este script publica la aplicación como un único archivo .exe para Windows (x64).
# El resultado no requerirá que los usuarios instalen .NET.

Write-Host "Iniciando la publicación de Escaneador..."

# Define la configuración y el Runtime Identifier (RID)
$Configuration = "Release"
$RuntimeID = "win-x64"

# Comando de .NET para publicar como self-contained y single-file
dotnet publish -c $Configuration -r $RuntimeID --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=false

# Comprueba si el comando fue exitoso
if ($LASTEXITCODE -ne 0) {
    Write-Host "Error: La publicación falló." -ForegroundColor Red
    exit 1
}

# Muestra la ubicación del archivo generado
$OutputPath = "bin\$Configuration\net9.0\$RuntimeID\publish\Escaneador.exe"
Write-Host "¡Publicación completada con éxito!" -ForegroundColor Green
Write-Host "El archivo .exe se encuentra en: $OutputPath"

# Abre el explorador de archivos en la carpeta de salida
explorer.exe (Join-Path $pwd.Path "bin\$Configuration\net9.0\$RuntimeID\publish")
