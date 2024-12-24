# SmartWeather.Communication Layer 📡

Welcome to the **Communication Layer** of SmartWeather! This layer manages all inter-service communication, message brokering, and data streaming. It is built around **Kafka** and **MQTT** to ensure scalability, reliability, and real-time processing.

---

## Table of Contents 📚

- [Overview](#overview-)
- [Components](#components-)
- [Configuration](#configuration-)
- [Key Features](#key-features-)
- [License](LICENSE)

---

## Overview 📝

The **Communication Layer** serves as the backbone for data flow in SmartWeather. It:
- Enables sensor data ingestion through MQTT brokers.
- Supports high-performance streaming and data processing via Kafka.
- Allow MQTT/Kafka link using kafka connector deployment.
- Ensures fault tolerance and scalability using cluster-based configurations.
- Provides multiple MQTT deployment options (simple and clustered setups).

---

## Components 🛠️

### 1. **MQTT Brokers** 📶
- **HiveMQ Cluster**: High-availability cluster setup with **HAProxy** for load balancing.
  - Configuration: `hivemq-cluster.yml`
- **Mosquitto Broker**: Lightweight alternative for simpler use cases.
  - Configuration: `mosquitto.yml`

### 2. **Kafka Cluster** 🧩
- Implements **KRaft mode** for broker and controller unification.
- 3-broker setup with:
  - **9 partitions** for load distribution.
  - **3 replication factors** to avoid data loss.
  - Automatic topic creation and initialization via init container.
- Configuration: `kafka-kraft-cluster.yml`

### 3. **MQTT-Kafka Connector** 🔗
- Transfers MQTT messages from topic `station/+/measure_point/+/toserver` to Kafka topic `smartweather.measure.data`.
- Ensures reliable integration between MQTT sensors and Kafka consumers.

---

## Configuration ⚙️

### Settings files

- MQTT connector config `mqtt_source_config.json` : 
    ```json
    {
        "name": "mqtt-source-toserver-connector",
        "config": {
            "tasks.max": "1",
            "mqtt.server.uri": "tcp://hivemq-proxy:1883",
            "mqtt.topics": "station/+/measure_point/+/toserver",
            "kafka.topic": "smartweather.measure.data",
        }
    }
    ```

---

## Key Features 🌟

- **Flexible MQTT Brokers**: Supports lightweight (Mosquitto) and clustered (HiveMQ) deployments.
- **Kafka Integration**: High-throughput messaging with replication and partitioning.
- **MQTT-Kafka Connector**: Bridges sensor data to the Kafka pipeline for further processing.
- **Scalable Architecture**: Cluster-ready configurations for high availability.
- **Fault Tolerance**: Replicated Kafka partitions and load-balanced MQTT ensure reliability.

---

## License 📜

SmartWeather is currently not open-source. Refer to the [License](../LICENSE) file for details.

---

For further details, explore other layers in the [main repository](../README.md). Happy coding! 😊

