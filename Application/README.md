# SmartWeather.Application Layer 🖥️

Welcome to the **Application Layer** of SmartWeather! This layer implements the business logic, exposes APIs, and provides real-time data streaming capabilities. It is written entirely in **C#** and follows an **n-tier architecture** with **exception-based programming**.

---

## Table of Contents 📚

- [Overview](#overview-)
- [Components](#components-)
- [Configuration](#configuration-)
- [Key Features](#key-features-)
- [License](LICENSE)

---

## Overview 📝

The **Application Layer** is designed to:
- Serve as the main entry point for external clients through HTTP and WebSocket APIs.
- Process and store data collected from weather stations.
- Handle authentication, user management, and data access.
- Provide utilities to consume and store streaming data from Kafka topics.

---

## Components 🛠️

### 1. **SmartWeather.Api** 🌐
- HTTP API exposing core functionalities, including:
  - **User Management**: Authentication and profile handling.
  - **Data Management**: Access to station and sensor data.
  - **Administrative Features**: Station setup and configurations.
  - Connects with **MySQL** for relational data storage.
  - Connects with **Elasticsearch** for historical data display.

### 2. **SmartWeather.Socket.Api** 🔌
- WebSocket API for real-time acquisition.
- Allows clients to subscribe to data streams and receive live updates.

### 3. **SmartWeather.Historian** 📊
- Kafka consumer for ingesting sensor data from the MQTT pipeline.
- Stores data in **Elasticsearch** for fast retrieval and analytics.
- Stateless design with **consumer groups** to ensure scalability and fault tolerance.

---

## Configuration ⚙️

### Settings files

Each project have their own configuration, related to the functionnalities they expose and elements they include from bellow libraries inclusion (dependency injection). 

Main configurations is located in appsettings.json file: 
- MySQL databases : 
    ```json
    "ConnectionStrings": {
        "SmartWeatherMaster": "Server=localhost;Database=smartweather;User=root;Password=rootpassword;Port=3306;SslMode=none;",
        "SmartWeatherLb": "Server=localhost;Database=smartweather;User=root;Password=rootpassword;Port=3307;SslMode=none;",
    }
    ```
- ElasticSearch database : 
    ```json
    "ConnectionStrings": {
        "ElasticsearchDb": "Host=http://localhost:9230;User=elastic;Password=elasticpassword"
    }
    ```
- MQTT communication : 
    ```json
    "Mqtt": {
        "ClientId": "SmartWeather.Service.MqttService",
        "EmitterId": "smart_weather_api_fa2b0808_5bc2_45dc_a81e_6979b9f4ef64",
        "BrokerAddress": "localhost",
        "BrokerPort": 1883,
        "Username": "swsm",
        "Password": "jl4[sv@*&c/ndjsl4-d=flbc!"
    }
    ```
- Kafka communication : 
    ```json
    "Kafka": {
        "BootstrapServers": "kafka-0:9092,kafka-1:9092,kafka-2:9092",
        "GroupId": "historian-group",
        "Topics": [
        "smartweather.measure.data"
        ]
    }
    ```
---

## Key Features 🌟

- **HTTP API**: Serves data and management tools to clients.
- **WebSocket API**: Provides real-time updates.
- **Historian Service**: Consumes and persists streaming data from Kafka.
- **Scalable Architecture**: Uses dependency injection and layered design.
- **Exception-Based Programming**: Ensures detailed error reporting and reliability.

---

## License 📜

SmartWeather is currently not open-source. Refer to the [License](../LICENSE) file for details.

---

For further details, explore other layers in the [main repository](../README.md). Happy coding! 😊

