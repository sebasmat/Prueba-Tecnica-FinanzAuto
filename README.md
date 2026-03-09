# 🛠️ Prueba Técnica FinanzAuto - API de Gestión de Productos

Este proyecto es una solución robusta desarrollada con **.NET 10** y **PostgreSQL** para la gestión integral de un catálogo de productos, categorías y proveedores, cumpliendo con los requerimientos técnicos de arquitectura y seguridad.

## 📋 Características del Proyecto
- **Arquitectura Limpia:** Separación clara entre capas de Aplicación, Núcleo e Infraestructura.
- **Seguridad:** Implementación de autenticación y autorización mediante **JWT (JSON Web Tokens)**.
- **Persistencia:** Uso de **Entity Framework Core** con migraciones automáticas.
- **Dockerizado:** Configuración lista para desplegar con Docker y Docker Compose.
- **Calidad de Código:** Pruebas unitarias y de integración desarrolladas con **xUnit**.

---

## 🚀 Instrucciones de Ejecución (Local)

### 1. Requisitos Previos
* Tener instalado [Docker Desktop](https://www.docker.com/products/docker-desktop/) y asegurarse de que esté en ejecución.

### 2. Clonar y Levantar el Proyecto
Para ejecutar la API y la base de datos sin configuraciones manuales, abra una terminal en la raíz del proyecto y ejecute:

```bash
# Clonar el repositorio
git clone [https://github.com/TU_USUARIO/TU_REPOSITORIO.git](https://github.com/TU_USUARIO/TU_REPOSITORIO.git)

# Entrar a la carpeta
cd "Prueba Tecnica FinanzAuto"

# Levantar servicios con Docker
docker-compose up --build
