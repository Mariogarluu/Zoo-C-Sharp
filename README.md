# Zoo Management System 🦁

Una aplicación de escritorio desarrollada en C# con WPF para gestionar zoológicos y sus animales.

## Descripción

Esta aplicación permite administrar una base de datos de zoológicos y los animales que habitan en ellos. Utiliza SQLite como sistema de gestión de base de datos y ofrece una interfaz gráfica intuitiva para realizar operaciones CRUD.

## Tecnologías Utilizadas

- **Lenguaje:** C# (.NET 8.0)
- **Framework UI:** WPF (Windows Presentation Foundation)
- **Base de Datos:** SQLite
- **IDE Recomendado:** Visual Studio 2022

## Características

- ✅ Gestión de Zoológicos (Crear, Leer, Actualizar, Eliminar)
- ✅ Gestión de Animales (Crear, Leer, Actualizar, Eliminar)
- ✅ Asociar animales a zoológicos
- ✅ Quitar animales de zoológicos
- ✅ Base de datos SQLite integrada
- ✅ Datos de prueba iniciales (Nueva York, Tokio, Berlín)

## Requisitos Previos

- Windows 10 o superior
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 (opcional, pero recomendado)

## Instalación

1. Clona el repositorio:
   ```bash
   git clone https://github.com/Mariogarluu/Zoo-C-Sharp.git
   ```

2. Navega al directorio del proyecto:
   ```bash
   cd Zoo-C-Sharp
   ```

3. Restaura las dependencias:
   ```bash
   dotnet restore
   ```

4. Compila el proyecto:
   ```bash
   dotnet build
   ```

5. Ejecuta la aplicación:
   ```bash
   dotnet run --project zoo
   ```

## Uso

### Gestión de Zoológicos
- **Agregar Zoo:** Escribe la ubicación en el cuadro de texto y haz clic en "Agregar Zoo"
- **Actualizar Zoo:** Selecciona un zoo, modifica el texto y haz clic en "Actualizar Zoo"
- **Eliminar Zoo:** Selecciona un zoo y haz clic en "Eliminar Zoo"

### Gestión de Animales
- **Agregar Animal:** Escribe el nombre en el cuadro de texto y haz clic en "Agregar animal al zoo"
- **Actualizar Animal:** Selecciona un animal, modifica el texto y haz clic en "Actualizar Animal"
- **Eliminar Animal:** Selecciona un animal y haz clic en "Eliminar animal"

### Asociación Zoo-Animal
- **Agregar Animal al Zoo:** Selecciona un zoo y un animal, luego haz clic en "Agregar Animal al Zoo"
- **Quitar Animal del Zoo:** Selecciona un zoo y un animal asociado, luego haz clic en "Quitar Animal"

## Estructura del Proyecto

```
Zoo-C-Sharp/
├── zoo/
│   ├── App.xaml              # Configuración de la aplicación
│   ├── App.xaml.cs           # Código de inicio de la aplicación
│   ├── MainWindow.xaml       # Interfaz gráfica principal
│   ├── MainWindow.xaml.cs    # Lógica de la ventana principal
│   ├── Zoo.db                # Base de datos SQLite
│   └── zoo.csproj            # Archivo de proyecto
└── zoo.sln                   # Archivo de solución
```

## Base de Datos

La aplicación utiliza tres tablas principales:

- **Zoo:** Almacena los zoológicos con su ubicación
- **Animal:** Almacena los animales con su nombre
- **AnimalZoo:** Tabla de relación muchos a muchos entre Zoo y Animal

## Licencia

Este proyecto es de código abierto y está disponible para uso educativo.

## Autor

Desarrollado por [Mariogarluu](https://github.com/Mariogarluu)
