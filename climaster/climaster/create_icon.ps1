# Script para crear un icono simple usando PowerShell
# Este script crea un icono básico de 32x32 píxeles

Add-Type -AssemblyName System.Drawing

# Crear un bitmap de 32x32
$bitmap = New-Object System.Drawing.Bitmap(32, 32)
$graphics = [System.Drawing.Graphics]::FromImage($bitmap)

# Fondo azul cielo con gradiente
$brush = New-Object System.Drawing.Drawing2D.LinearGradientBrush(
    (New-Object System.Drawing.Point(0, 0)),
    (New-Object System.Drawing.Point(0, 32)),
    [System.Drawing.Color]::FromArgb(100, 150, 230),
    [System.Drawing.Color]::FromArgb(50, 100, 180)
)
$graphics.FillRectangle($brush, 0, 0, 32, 32)

# Dibujar un sol simple (círculo amarillo)
$sunBrush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(255, 220, 80))
$graphics.FillEllipse($sunBrush, 8, 8, 16, 16)

# Guardar como ICO
$icon = [System.Drawing.Icon]::FromHandle($bitmap.GetHicon())
$fileStream = [System.IO.File]::Create("icon.ico")
$icon.Save($fileStream)
$fileStream.Close()

Write-Host "Icono creado exitosamente: icon.ico"

# Limpiar recursos
$graphics.Dispose()
$bitmap.Dispose()
$sunBrush.Dispose()
$brush.Dispose()
