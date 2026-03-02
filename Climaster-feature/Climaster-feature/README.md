# CliMaster Widget Designer ???

**Desktop Application** para diseñar widgets personalizados de Android para la aplicación móvil CliMaster.

## ?? Funcionalidades Principales

### 1. **Editor Visual de Widgets**
- Interfaz WYSIWYG (What You See Is What You Get)
- **Vista previa en tiempo real** del widget en simulador Android
- Vista previa del JSON generado
- Estilo Glassmorphism moderno

### 2. **Elementos Disponibles**
- ??? **Temperatura Actual** - Muestra la temperatura actual
- ?? **Condición Actual** - Texto descriptivo del clima
- ?? **Humedad** - Porcentaje de humedad
- ?? **Viento** - Velocidad del viento
- ? **Divisor Horizontal** - Separador visual
- ?? **Previsión Diaria** - Pronóstico de múltiples días
- ? **Previsión Horaria** - Pronóstico por horas

### 3. **Configuración Personalizable**
- **Widget ID**: Identificador único del widget
- **Color de Fondo**: Color hexadecimal con transparencia (ej: #33FFFFFF)
- **Radio de Esquinas**: 0-50px para bordes redondeados
- **Ordenamiento**: Mueve elementos hacia arriba/abajo en el layout

### 4. **Exportación y Compartir** ?

#### **?? QR Code con JSON Embebido** (Recomendado) ??
- **No requiere permisos de administrador**
- Genera un código QR que contiene directamente el JSON del widget
- El dispositivo Android lee el QR y obtiene la configuración completa
- Ideal para widgets de tamaño pequeño a mediano

#### **?? Guardar Archivo JSON**
- Exporta la configuración a un archivo .json
- Transfiere manualmente al dispositivo Android

#### **?? Servidor HTTP Local** (Opcional - Requiere Admin)
- Inicia un servidor HTTP en tu red local
- Genera QR con URL al archivo JSON
- Útil para widgets muy grandes que no caben en un QR
- **Requiere ejecutar la aplicación como Administrador**

### 5. **Preview Android en Tiempo Real**
- Simulador de pantalla Android
- **Actualización automática** al añadir/eliminar/reordenar elementos
- Vista previa exacta de cómo se verá en el dispositivo
- Incluye barra de estado simulada

## ?? Cómo Usar

### 1. Diseñar el Widget
1. Abre la aplicación
2. Configura el **Widget ID**, **Color de Fondo** y **Radio de Esquinas**
3. Añade elementos desde el panel izquierdo
4. **Observa la vista previa** en tiempo real en el simulador Android
5. Reordena los elementos con los botones ?? ??
6. Elimina elementos con el botón ???

### 2. Exportar el Widget

#### **Opción A: QR Code con JSON (Recomendado)** ??
1. Click en **"?? Generar QR con JSON"**
2. El código QR aparece automáticamente en el panel derecho
3. Desde Android:
   - Abre la aplicación CliMaster
   - Selecciona "Escanear Widget QR"
   - Escanea el código QR mostrado en pantalla
   - El widget se importa automáticamente

**Ventajas:**
- ? No requiere permisos especiales
- ? Funciona sin conexión a internet
- ? No necesita estar en la misma red
- ? Rápido y sencillo

**Limitaciones:**
- ?? Widgets muy grandes pueden no caber en el QR
- ?? Máximo ~2900 caracteres en el JSON

#### **Opción B: Guardar Archivo JSON**
1. Click en **"?? Guardar JSON"**
2. Elige la ubicación del archivo
3. Transfiere manualmente a Android (USB, email, etc.)

#### **Opción C: Servidor HTTP Local**
1. **Ejecuta la aplicación como Administrador** (clic derecho > Ejecutar como administrador)
2. Click en **"?? Iniciar Servidor"**
3. La aplicación:
   - Inicia un servidor HTTP en tu red local
   - Genera automáticamente un código QR con la URL
   - Muestra la URL (ej: `http://192.168.1.100:8080/widget.json`)
4. Desde Android:
   - Escanea el código QR mostrado en pantalla
   - O introduce manualmente la URL
   - El widget se descarga automáticamente

**Importante:** El servidor permanece activo hasta que:
- Presiones el botón **"?? Detener Servidor"**
- Cierres la aplicación (se detiene automáticamente)

### 3. Importar en Android

```kotlin
// En la app Android con Jetpack Glance

// Método 1: Desde QR con JSON embebido
val widgetConfig = parseQRCodeJSON(qrContent)
renderWidget(widgetConfig)

// Método 2: Desde URL (servidor HTTP)
val widgetConfig = downloadWidgetConfig(qrUrl)
renderWidget(widgetConfig)
```

## ?? Características del UI

### Glassmorphism Design
- Fondo oscuro (#0A0E27)
- Paneles con efecto de cristal (#1A1E3F)
- Bordes con transparencia (#33FFFFFF)
- Sin efectos de desenfoque (mejor legibilidad)
- Botones con hover effects

### Layout Responsivo de 4 Columnas
1. **Panel Izquierdo** (300px): Configuración y elementos
2. **Panel Central** (Flexible): Editor de layout
3. **Panel Android Preview** (350px): Simulador del widget
4. **Panel Derecho** (350px): QR Code + Vista previa JSON

## ?? Tecnologías

- **.NET 8.0** - Framework principal
- **WPF** - Windows Presentation Foundation
- **C# 12** - Lenguaje de programación
- **MVVM** - Patrón de diseño
- **QRCoder** - Generación de códigos QR
- **System.Text.Json** - Serialización JSON

## ?? Formato JSON del Widget

```json
{
  "widgetId": "tokyo_summer_widget_v1",
  "size": {
    "width": "match_parent",
    "height": "wrap_content"
  },
  "background": {
    "type": "glassmorphism",
    "color": "#33FFFFFF",
    "cornerRadius": 24
  },
  "layout": [
    {
      "type": "current_temp",
      "fontSize": 48,
      "alignment": "center"
    },
    {
      "type": "current_condition_text",
      "fontSize": 20,
      "alignment": "center"
    },
    {
      "type": "horizontal_divider"
    },
    {
      "type": "daily_forecast_row",
      "days": 3
    }
  ]
}
```

## ??? Arquitectura

```
Climaster-feature/
??? Models/
?   ??? WidgetConfiguration.cs    # Modelo de datos del widget
??? ViewModels/
?   ??? ViewModelBase.cs          # Base para MVVM
?   ??? MainViewModel.cs          # Lógica principal
?   ??? RelayCommand.cs           # Comandos ICommand
?   ??? AsyncRelayCommand.cs      # Comandos asíncronos
??? Services/
?   ??? QRCodeService.cs          # Generación de códigos QR
?   ??? LocalServerService.cs     # Servidor HTTP local
??? Views/
?   ??? AndroidWidgetPreview.xaml # Preview visual del widget
??? Converters/
?   ??? ValueConverters.cs        # Conversores WPF
??? Helpers/
    ??? BitmapHelper.cs           # Conversión de imágenes
```

## ?? Notas Técnicas

### Dependencias NuGet
```xml
<PackageReference Include="QRCoder" Version="1.7.0" />
<PackageReference Include="System.Drawing.Common" Version="10.0.3" />
```

### QR Code con JSON Embebido
- El JSON se serializa en formato compacto (sin indentación)
- Capacidad máxima del QR: ~2900 caracteres
- Nivel de corrección de errores: Q (25%)
- No requiere conexión de red
- No requiere permisos especiales

### Servidor HTTP Local
- Puerto por defecto: **8080**
- Si está ocupado, incrementa automáticamente (8081, 8082...)
- **Permanece activo** usando `CancellationToken`
- Soporta múltiples peticiones simultáneas
- Headers CORS configurados para acceso desde Android
- **REQUIERE permisos de administrador** en Windows

### Vista Previa Android
- Se actualiza **automáticamente** al modificar elementos
- Usa `ObservableCollection.CollectionChanged` para detectar cambios
- Renderiza elementos dinámicamente según el tipo
- Simula datos de ejemplo (24°, "Parcialmente nublado", etc.)

### Formato de Colores
- Formato HEX con alpha: `#AARRGGBB`
- Ejemplo: `#33FFFFFF` (blanco con 20% de opacidad)

## ?? Troubleshooting

**Error "Acceso denegado" al iniciar servidor:**
- **Solución:** Usa el botón **"?? Generar QR con JSON"** que no requiere permisos
- **Alternativa:** Ejecuta la aplicación como Administrador
- El QR con JSON embebido funciona igual de bien para la mayoría de casos

**El QR no se genera:**
- Verifica que hayas pulsado el botón "?? Generar QR con JSON"
- Si el JSON es muy grande (>2900 caracteres), prueba reducir elementos
- Si persiste, usa el método de guardar JSON manualmente

**Android no puede leer el QR:**
- Asegúrate de que la app Android tenga permisos de cámara
- Verifica que el QR esté bien iluminado
- Prueba acercando/alejando la cámara

**La vista previa no se actualiza:**
- Esto está corregido mediante `CollectionChanged`
- Si persiste, reinicia la aplicación

## ? Novedades v1.2

- ?? **QR Code con JSON embebido** - No requiere servidor ni permisos
- ? Manejo robusto de errores con mensajes informativos
- ? UI mejorado con estados claros del QR
- ? Soporte para ambos métodos (QR directo + Servidor HTTP)
- ? Tooltips informativos en botones
- ? Mejor organización de opciones de exportación

## ?? Integración con Android

El widget diseñado en esta aplicación será renderizado en Android usando:
- **Jetpack Glance** - Para widgets de pantalla de inicio
- **Kotlin** - Lenguaje de la app móvil
- **ZXing** - Librería para escanear códigos QR
- **Pirate Weather API** - Datos meteorológicos reales
- **Groq API (Llama 3.3)** - Insights AI

## ????? Autor

Desarrollado para el ecosistema **CliMaster** ???

## ?? Licencia

Proyecto privado - CliMaster Team
