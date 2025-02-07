# SmartWeather.Libraries (libs) 📚

Welcome to the **Libraries** folder of SmartWeather! This section contains reusable code components written in C# to support various applications, from station mocker to api. It follows an **n-tier architecture** for scalability, maintainability, and code reusability. Currently implemented with exception-based method, will probably be refactor to match common programming standards.

---

## Table of Contents 📋

- [Overview](#overview-)
- [Structure](#structure-)
- [Projects](#projects-)
- [Key Features](#key-features-)
- [License](LICENSE)

---

## Overview 📝

This folder houses C# libraries grouped into three primary layers:
1. **Entities** - Definitions for business objects and exceptions.
2. **Repositories** - Handles data persistence and database interactions.
3. **Services** - Implements business logic and exposes functionalities for APIs.

The libraries promote code reuse and simplify integration into other application components, such as APIs and data processing runners.

---

## Structure 🏗️

| Folder                        | Description                                                                                         |
|-------------------------------|-----------------------------------------------------------------------------------------------------|
| `SmartWeather.CSharpLibraries.sln` | Visual Studio solution for managing library projects.                                                |
| `SmartWeather.Entities`       | Contains domain models and exception handling for entity definitions.                              |
| `SmartWeather.Repositories`   | Interfaces and implementations for accessing MySQL and Elasticsearch databases.                     |
| `SmartWeather.Services`       | Business logic services providing high-level functionality and abstracting repositories.             |

---

## Projects 🛠️

### 1. **SmartWeather.Entities** 🧱
- **Purpose**: Define core business models (User, Station, Component, MeasurePoint, MeasureData).
- **Features**:
  - Centralized exception handling (e.g., `EntityCreationException`, `EntityFetchingException`).
  - Simplifies debugging and enforces consistency across data structures.

### 2. **SmartWeather.Repositories** 🗄️
- **Purpose**: Abstract database interactions.
- **Supported Databases**:
  - MySQL: Master-slave replication via proxy.
  - Elasticsearch: Document storage for historical data.
- **Features**:
  - Dynamic connection handling (read vs write).
  - Scalable design for multi-database support.

### 3. **SmartWeather.Services** ⚙️
- **Purpose**: Encapsulate complex operations and workflows.
- **Features**:
  - Integrates repositories seamlessly.
  - Provides utility runners like `Historian` for real-time data processing.
  - Supports dependency injection for flexibility and testing.

---

## Key Features 🌟

- **Modular Architecture**: Clean separation between entities, repositories, and services.
- **Exception-Based Programming**: Error handling is built into all layers.
- **Dependency Injection**: Simplifies integration and testing.
- **Scalable Design**: Prepared for future enhancements and scaling requirements.

---

## License 📜

SmartWeather is currently not open-source. Refer to the [License](../LICENSE) file for details.

---

Explore the other layers in the [main repository](../README.md). Happy coding! 😊

