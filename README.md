# SmartWeather 🌦️

SmartWeather is a microservices-based weather data acquisition system. It is designed to collect, process, and display weather data without relying on any existing weather stations. Instead, it integrates its own custom-built acquisition modules, a robust backend, and an interactive web interface.

---

## Table of Contents 📚

- [Overview](#overview)
- [Architecture](#architecture)
- [Quick Start](#quick-start)
- [Directory Structure](#directory-structure)
- [Technologies](#technologies)
- [Contributors](#contributors)
- [License](#license)

---

## Overview 📝

SmartWeather is built as a **monorepo** with multiple layers to handle data acquisition, processing, storage, and presentation. The application supports **containerization** using Docker Compose.Kubernetes orchestration will maybe appear in the future but not immediatly.

Key Features:
- **Embedded Systems Support**: C++ code runs on ESP32 microcontrollers for data acquisition.
- **Microservices Architecture**: Each layer is modular to enable scaling and features additions.
- **Real-Time Data Processing**: Kafka and MQTT enable seamless streaming and communication.
- **Flexible Storage Solutions**: MySQL clusters for structured data and Elasticsearch for document-oriented data.
- **Interactive Visualization**: Kibana, Kafka-ui and a web client for monitoring and administration.

---

## Architecture 🏗️

Most of SmartWeather services adopts an **n-tier architecture** with the following layers, for C# apps you will find theses layers in the [/libs](./libs/README.md) folder:

1. **Entities** - Definition for the base entities that will structure application logic.
2. **Repositories** - Persistence layer interface to abstract from databases implementation.
3. **Services** - Main entrypoint for app integration, provides main functionalities to again abstract from repositories handling.

Each layer represent a library, it also exist for C++ application but it is not currently delocalized in the [/libs](./libs/README.md) folder.

---

## Quick Start 🚀

SmartWeather.Project is made to be ultra portable, regardless of the complexity the project embded. Basically you will only need docker to run it, and a single command. If it's not the case, don't hesitate to open an Issue.

### Prerequisites 🛠️
- **Docker** and **Docker Compose** installed.
- **.NET SDK** and **Node.js** (for development).

### Deployment ⚡
```bash
# Clone the repository
git clone https://github.com/your-repo/SmartWeather.git
cd SmartWeather.Project

# Start the entire application
docker compose -f ./app.yml up -d
```

### Accessing Services 🌐
- Web Client: [http://localhost:3000](http://localhost:3000)
- Kafka UI: [http://localhost:8080](http://localhost:8080)
- Kibana: [http://localhost:5601](http://localhost:5601)

---

## Directory Structure 🗂️

| Folder                   | Description                                                                                           |
|--------------------------|-------------------------------------------------------------------------------------------------------|
| `/Acquisition`           | Embedded software in C++ and mock station in C# for data collection.                                 |
| `/Communication`         | MQTT brokers (HiveMQ, Mosquitto) and Kafka clusters for message handling.                            |
| `/Application`           | C# applications including HTTP APIs, WebSocket APIs, and data historian services.                    |
| `/Persistence`           | Databases: MySQL clusters and Elasticsearch for data storage.                                        |
| `/Presentation`          | Visualization tools (Kibana, Kafka UI) and a web client built with React/Next.js.                    |
| `/libs`                  | Shared C# libraries for entities, repositories, and services.                                        |

---

## Technologies 🧰

- **Programming Languages**: C++ (embedded), C# (backend), TypeScript (frontend).
- **Databases**: MySQL (clustered), Elasticsearch.
- **Messaging Systems**: Kafka, MQTT.
- **Containerization**: Docker, Docker Compose.
- **Frontend Framework**: React.js (Next.js).
- **Monitoring Tools**: Kibana, Kafka UI.
- **Orchestration**: Kubernetes (planned).

---

## Contributors 👥

- **[lucas066001]** - Project Lead
- **[matthias-goupil]** - Front-end developer

---

## License 📜

SmartWeather is currently not made to be open-source nor public, see the [License](LICENSE) for details.

---

## Next Steps 🔧

Explore each subdirectory for more detailed documentation:

- [Acquisition](./Acquisition/README.md)
- [Communication](./Communication/README.md)
- [Application](./Application/README.md)
- [Persistence](./Persistence/README.md)
- [Presentation](./Presentation/README.md)
- [Libraries](./libs/README.md)

Happy coding! 😊