# Left 4 Dead 2 - Verificador de Cuentas

Este programa es una herramienta antitrampas diseñada para torneos de Left 4 Dead 2. Su función principal es verificar la cantidad de cuentas de usuario locales en una computadora para asegurar que los jugadores no excedan el límite permitido (por ejemplo, 3 cuentas).

## Características

- **Detección de Cuentas**: Escanea el sistema en busca de todas las cuentas de usuario locales.
- **Conteo**: Informa el número total de cuentas encontradas.
- **Validación**: (Futura implementación) Compara el número de cuentas con un límite predefinido y alerta si se excede.

## Cómo Compilar

Para compilar este proyecto y generar un archivo `.exe`, necesitarás tener instalado el SDK de .NET. Luego, puedes usar el siguiente comando en la terminal:

```bash
dotnet build -c Release
```

El archivo ejecutable se encontrará en la carpeta `bin/Release/netX.X/`.
