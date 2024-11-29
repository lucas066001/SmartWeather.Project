#************************
#***** Image section ****
#************************

gen-imgs: gen-acq-imgs gen-app-imgs

#Application layer build
gen-app-imgs: gen-api gen-socketapi gen-historian
gen-api:
	docker build -t smartweather.application.api -f "./Application/SmartWeather.Api/Dockerfile" "./Application"
gen-socketapi:
	docker build -t smartweather.application.socketapi -f "./Application/SmartWeather.Socket.Api/Dockerfile" "./Application"
gen-historian:
	docker build -t smartweather.application.historian -f "./Application/SmartWeather.Historian/Dockerfile" "./Application"

#Acquisition layer build
gen-acq-imgs: gen-stationmocker
gen-stationmocker:
	docker build -t smartweather.acquisition.stationmocker -f "./Acquisition/SmartWeather.StationMocker/Dockerfile" "./"

gen-help:
	@echo "Available commands:"
	@echo "  -Build multiple images at once"
	@echo "     make gen-imgs           - Build all Docker images (application and acquisition layers)."
	@echo "     make gen-acq-imgs       - Build Docker images for the acquisition layer."
	@echo "     make gen-app-imgs       - Build Docker images for the application layer."
	@echo "  -Build single images"
	@echo "     make gen-api            - Build the Docker image for the SmartWeather API."
	@echo "     make gen-socketapi      - Build the Docker image for the SmartWeather Socket API."
	@echo "     make gen-historian      - Build the Docker image for the SmartWeather Historian."
	@echo "     make gen-stationmocker  - Build the Docker image for the SmartWeather Station Mocker."

#************************
#***** Kube section *****
#************************

init-kube: gen-kube-ns

gen-kube-ns:
	kubectl get namespace smartweather-persistence >NUL 2>NUL || kubectl create namespace smartweather-persistence
	kubectl get namespace smartweather-acquisition >NUL 2>NUL || kubectl create namespace smartweather-acquisition
	kubectl get namespace smartweather-application >NUL 2>NUL || kubectl create namespace smartweather-application
	kubectl get namespace smartweather-communication >NUL 2>NUL || kubectl create namespace smartweather-communication
	kubectl get namespace smartweather-presentation >NUL 2>NUL || kubectl create namespace smartweather-presentation

gen-kube-apps:
	kubectl apply -f ./Persistence/Mysql/mysql.yaml -n smartweather-persistence
	kubectl apply -f ./Persistence/Elasticsearch/elasticsearch.yaml -n smartweather-persistence
	kubectl apply -f ./Communication/Mqtt/mqtt-proxy.yaml -n smartweather-communication