# SmartWeather.Acquisition Layer 🎛️

Welcome to the **Acquisition Layer** of SmartWeather! This layer is responsible for collecting weather data directly from custom-built stations or mock environments. It includes both **embedded software** for ESP32 microcontrollers and a **mock station simulator** for testing purposes.

---

## Table of Contents 📚

- [Overview](#overview-)
- [Components](#components-)
- [Quick Start](#quick-start-)
- [Configuration](#configuration-)
- [Key Features](#key-features-)
- [License](LICENSE)

---

## Overview 📝

The **Acquisition Layer** handles:
- Collecting real-time data from physical weather stations (ESP32-based).
- Simulating weather station behavior with a C#-based **StationMocker** for testing and development.

---

## Components 🛠️

### 1. **SmartWeather.EmbeddedSoftware** (C++)
- Runs on **ESP32 microcontrollers**.
- Provides a **web interface** for configuring network access.
- Connects to the desired network and communicates via **MQTT** to SmartWeather.Communication layer.
- Sends configuration requests to the server and continuously transmits sensor data.

### 2. **SmartWeather.StationMocker** (C#)
- Simulates weather station behavior for testing and development purposes.
- Generates realistic, configurable sensor data and sends it via MQTT.
- Useful for testing data pipelines and storage without physical hardware.

---

## Quick Start 🚀

### Prerequisites 🛠️
- **ESP-IDF** or **Arduino IDE** (for ESP32 development).
- **.NET SDK** (for StationMocker).
- **Docker** (for running infrastructure components).

### Deployment ⚡

#### Embedded Software
1. Open the `SmartWeather.EmbeddedSoftware` project in your IDE.
2. Configure  MQTT settings.
3. Flash the code to your ESP32 device.

#### StationMocker
1. Update docker-compose.yml to configure admin token for your mocker.
2. Rebuild image.
3. Launch again the mocker and it will start generate data.

---

## Configuration ⚙️

### Embedded Software (ESP32)
Update the `MqttConfigConstants.cpp` file with:
```cpp
  const IPAddress MQTT_IP(192, 168, 43, 199); // Mqtt cluster localisation
```

### StationMocker Environment Variables
```yaml
environment:
  API_URL: "http://smart-weather-api:8081/api/Station/Update"
  MAX_ERROR_RATE: 10
  STATION_NB: 50
  DATA_FREQ: 980
  ADMIN_TOKEN: "your-token"
```

---

## Key Features 🌟

- **Embedded Support**: Designed for ESP32 microcontrollers.
- **Mock Environment**: Test the full pipeline without hardware dependencies.
- **Real-Time Streaming**: Uses MQTT for fast, reliable data transmission.
- **Configurable Simulation**: Adjust error rates, data frequency, and station count.

---

## License 📜

SmartWeather is currently not open-source. Refer to the [License](../LICENSE) file for details.

---

For further details, check out other layers in the [main repository](../README.md). Happy coding! 😊

