# MediaVault — Gestor de colección de anime y videojuegos

## 1. Información general

- **Estudiante:** Omar Navarro Dominguez
- **Descripción:** aplicación de escritorio para administrar una colección personal de anime y videojuegos.
- **Necesidad que resuelve:** permite mantener en un solo lugar títulos, géneros, estado de avance, calificación y un dato propio de cada categoría.
- **Objetivo:** demostrar el uso de POO, Avalonia UI y ADO.NET con SQLite mediante una aplicación CRUD funcional.

## 2. Funcionalidades

- Registrar anime y videojuegos.
- Consultar todos los elementos guardados y seleccionar uno para ver sus datos en el formulario.
- Modificar el elemento seleccionado y guardar los cambios.
- Eliminar el elemento seleccionado.
- Validar que el título, género y dato específico no estén vacíos, que la categoría y el estado sean válidos y que la calificación sea un número entero entre 1 y 10.
- Mostrar mensajes de resultado o de validación en la ventana.
- Conservar la colección en SQLite entre ejecuciones.

Para un anime, el dato específico es episodios/estudio; para un videojuego, es plataforma. Los estados permitidos son **Pendiente**, **En progreso** y **Completado**.

## 3. Tecnologías utilizadas

- .NET 10 SDK y C#.
- Avalonia UI 12.1.5 para la interfaz de escritorio.
- ADO.NET mediante `Microsoft.Data.Sqlite` 11.0.0.
- SQLite como base de datos local.
- CommunityToolkit.Mvvm 8.4.2 está declarado como dependencia del proyecto.

## 4. Estructura del proyecto y POO

```text
MiProyecto/
├── App.axaml, App.axaml.cs, Program.cs
├── Data/
│   ├── ConexionSQLite.cs
│   └── ElementoDAO.cs
├── Models/
│   ├── Elemento.cs
│   ├── Anime.cs
│   └── Videojuego.cs
├── Services/
│   └── ColeccionServicio.cs
├── Views/
│   ├── MainWindow.axaml
│   └── MainWindow.axaml.cs
├── evidencias/
└── MiProyecto.csproj
```

- `ElementoColeccion` es la clase abstracta base y contiene las propiedades comunes, la validación encapsulada de la calificación y el método abstracto `ObtenerResumen`.
- `Anime` y `Videojuego` heredan de `ElementoColeccion`, implementan sus datos específicos y sobrescriben el método abstracto. Esto aplica herencia y polimorfismo según las dos categorías del dominio.
- `ColeccionServicio` valida los datos y coordina las operaciones solicitadas.
- `ElementoDAO` ejecuta las operaciones SQL parametrizadas y convierte filas en objetos del modelo.
- `ConexionSQLite` configura y abre las conexiones e inicializa la tabla.
- `MainWindow.axaml` define la interfaz; `MainWindow.axaml.cs` conecta los eventos de botones y selección con el servicio y muestra los mensajes.

## 5. Instalación de dependencias

1. Instalar el **SDK de .NET 10** desde el sitio oficial de .NET.
2. Clonar el repositorio y entrar en la carpeta que contiene `MiProyecto.csproj`:

   ```powershell
   git clone <URL_DEL_REPOSITORIO>
   cd <carpeta-del-repositorio>
   ```

3. Restaurar los paquetes declarados en el archivo del proyecto:

   ```powershell
   dotnet restore
   ```

No se requiere instalar SQLite por separado: el proveedor `Microsoft.Data.Sqlite` está incluido como dependencia NuGet.

## 6. Ejecución

Desde la carpeta del proyecto:

```powershell
dotnet run
```

La ventana de MediaVault debe abrirse. También se puede compilar antes de iniciar con `dotnet build`.

## 7. Base de datos

La aplicación crea automáticamente `Elementos` al iniciar si la tabla aún no existe. Guarda `Id`, `Tipo`, `Titulo`, `Genero`, `Estado`, `Calificacion` y `DatoExtra`.

La base de datos se crea en la carpeta de datos locales del usuario, dentro de `MediaVault/mediavault.db` (en Windows, normalmente `%LOCALAPPDATA%\MediaVault\mediavault.db`). Así los registros no dependen de la carpeta de compilación `bin/`.

`ConexionSQLite` crea la conexión ADO.NET y la tabla. `ElementoDAO` contiene las operaciones SQL `INSERT`, `SELECT`, `UPDATE` y `DELETE`; las consultas que reciben datos de usuario usan parámetros. `ColeccionServicio` valida las entradas antes de enviarlas al DAO. Las conexiones, comandos y lectores se liberan con `using`.

## 8. Pruebas funcionales y evidencias

Las evidencias deben ser capturas reales tomadas al ejecutar la aplicación. La carpeta `evidencias/` está preparada, pero aún no contiene capturas; no se incluyen imágenes simuladas. Después de probar, guardar las imágenes allí y añadirlas a esta sección con una descripción de cada resultado.

| Prueba | Acción para demostrarla | Resultado esperado | Evidencia |
|---|---|---|---|
| Registro | Guardar un anime o videojuego con datos válidos | Aparece en la lista | Pendiente de captura |
| Consulta | Iniciar la aplicación o revisar la lista tras guardar | Se muestran los registros persistidos | Pendiente de captura |
| Actualización | Seleccionar un registro, cambiar un dato y actualizar | La lista muestra el dato nuevo | Pendiente de captura |
| Eliminación | Seleccionar un registro y eliminarlo | Desaparece de la lista | Pendiente de captura |
| Validación | Intentar guardar con título vacío o calificación no numérica/fuera de rango | Se muestra un mensaje y la aplicación sigue abierta | Pendiente de captura |
| Persistencia | Cerrar y abrir de nuevo después de guardar | El registro permanece disponible | Pendiente de captura |

## 9. Problemas encontrados y soluciones

- **La aplicación no compilaba al iniciar:** la clase de ventana está declarada en el espacio de nombres `MediaVault`, pero el arranque intentaba utilizar `MiProyecto.Views.MainWindow`. Se corrigió el arranque para instanciar la ventana existente.
- **La base de datos dependía de la carpeta de ejecución:** almacenar SQLite junto a los binarios podía perder o separar los datos al compilar o publicar. Ahora se guarda en la carpeta de datos locales del usuario y se crea automáticamente.
