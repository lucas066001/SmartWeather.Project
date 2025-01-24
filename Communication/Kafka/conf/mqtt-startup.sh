#!/bin/bash

echo "Kafka Connect is up and running."

# Installer les connecteurs MQTT (source et sink)
echo "Installing MQTT connectors..."
confluent-hub install --no-prompt confluentinc/kafka-connect-mqtt:latest

# Lancer Kafka Connect en arrière-plan
echo "Starting Kafka Connect..."
connect-distributed /etc/kafka/connect-distributed.properties &

# Attendre que Kafka Connect soit complètement démarré
echo "Waiting for Kafka Connect to be ready..."
while ! curl -s http://localhost:8083/ >/dev/null; do
  echo "Waiting for Kafka Connect to start..."
  sleep 5
done

sleep 1

# Créer le connecteur source MQTT vers Kafka
# Vérifier si le connecteur existe déjà
if curl -s http://localhost:8083/connectors/mqtt-source-toserver-connector >/dev/null; then
  echo "Connector 'mqtt-source-toserver-connector' already exists. Skipping creation."
else
  echo "Creating MQTT source connector..."
  curl -X POST -H "Content-Type: application/json" --data @/config/mqtt_source_config.json http://localhost:8083/connectors
fi
# Garder le script actif en attendant les processus enfants (comme connect-distributed)
wait
