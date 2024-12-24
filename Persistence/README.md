# Persistence Layer 🗄️

Welcome to the **Persistence Layer** of SmartWeather! This layer is responsible for data storage, providing both relational and document-based databases to handle the application's diverse data requirements.

---

## Table of Contents 📚

- [Overview](#overview-)
- [Components](#components-)
- [Key Features](#key-features-)
- [License](LICENSE)

---

## Overview 📝

The **Persistence Layer** provides scalable and high-availability storage solutions for SmartWeather, including:
- **MySQL**: Clustered relational database for structured data.
- **Elasticsearch**: Document-oriented database for sensor data and logs.
- **Influxdb**: TimeSeries database, relica of previous technical choice, not used, will be removed.

---

## Components 🛠️

### 1. **MySQL Database** 🗃️
- **Clustered Setup**:
  - **1 Master Node**: Handles write operations.
  - **2 Slave Nodes**: Handle read operations.
  - **ProxySQL**: Routes traffic between master and slaves.
  - Configuration: `mysql-cluster.yml`

- **Single Node Setup**:
  - Lightweight alternative for development and testing.
  - Configuration: `mysql-single-node.yml`

### 2. **Elasticsearch Database** 📊
- Stores historical sensor data for analysis and visualization.
- Supports full-text search and complex queries.
- Configuration: `elasticsearch.yml`

---

## Key Features 🌟

- **Scalable MySQL Cluster**: Provides high availability and load balancing for transactional data.
- **Flexible Elasticsearch Storage**: Ideal for time-series data and advanced queries.
- **Multi-Mode Deployment**: Choose between clustered and single-node setups based on requirements.
- **Fault Tolerance**: Ensures reliability with replication and failover mechanisms.

---

## License 📜

SmartWeather is currently not open-source. Refer to the [License](../LICENSE) file for details.

---

For further details, explore other layers in the [main repository](../README.md). Happy coding! 😊

