
# MvcAlimentosApp
Aplicación ASP.NET Core MVC para gestión de alimentos (CRUD) con Login y roles (Admin/Usuario).
El proyecto MvcAlimentosApp es una aplicación web desarrollada con ASP.NET Core MVC cuyo objetivo principal es la gestión de alimentos mediante operaciones CRUD (Crear, Leer, Actualizar y Eliminar). La aplicación permite administrar información de alimentos de manera organizada, segura y eficiente, siguiendo la arquitectura Modelo–Vista–Controlador (MVC).

La solución utiliza Entity Framework Core para la persistencia de datos y una base de datos relacional, permitiendo el acceso y manipulación de la información de forma estructurada. Además, se integra el sistema de inyección de dependencias propio de ASP.NET Core para mejorar el desacoplamiento y la mantenibilidad del código.

Como parte de las mejoras del taller formativo, el proyecto fue refactorizado aplicando buenas prácticas de desarrollo, incluyendo principios SOLID y patrones de diseño. En particular, se implementaron los principios Dependency Inversion y Open/Closed, así como el patrón Repository, lo que permitió separar la lógica de acceso a datos, la lógica de negocio y la lógica de presentación.

La arquitectura final se organiza en capas bien definidas:

Controllers: gestionan las solicitudes HTTP y la interacción con las vistas.

Services: contienen la lógica de negocio.

Repositories: encapsulan el acceso a datos.

Interfaces: definen contratos para reducir el acoplamiento.

Gracias a esta estructura, el proyecto es más escalable, mantenible y fácil de extender, manteniendo todas sus funcionalidades originales completamente operativas.
