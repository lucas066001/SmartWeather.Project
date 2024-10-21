#!/bin/bash

echo "Kafka Connect is up and running."

# Installer les connecteurs MQTT (source et sink)
echo "Installing MQTT connectors..."
confluent-hub install --no-prompt confluentinc/kafka-connect-mqtt:latest

# Lancer Kafka Connect
echo "Starting Kafka Connect..."
exec connect-distributed /etc/kafka/connect-distributed.properties

# Créer le connecteur source MQTT vers Kafka
# echo "Creating MQTT source connector..."
# curl -X POST -H "Content-Type: application/json" --data @/config/mqtt_source_config.json http://localhost:8083/connectors

# # Créer le connecteur sink Kafka vers MQTT
# echo "Creating MQTT sink connector..."
# curl -X POST -H "Content-Type: application/json" --data @/config/mqtt_sink_config.json http://localhost:8083/connectors
