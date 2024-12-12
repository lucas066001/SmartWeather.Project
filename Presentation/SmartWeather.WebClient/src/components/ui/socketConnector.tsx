"use client"
import { getToken } from '@/lib/tokenManager';
import { Status } from '@/services/dtos/api';
import { MeasurePointDataDto, MonitoringRequest } from '@/services/dtos/socket';
import { signalRService } from '@/services/socket_service';
import { StationDto, StationType } from '@/services/station/dtos/station_response';
import { getAllStation } from '@/services/station/station_service';
import React, { useEffect, useState } from 'react'
import { LineChart, Line, CartesianGrid, XAxis, YAxis } from 'recharts';
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import { Icon } from 'leaflet';

function socketConnector() {
    const [measurePoints, setMeasurePoints] = useState<MeasurePointDataDto[]>([]);
    const [stations, setStations] = useState<StationDto[]>([]);
    const [filterId, setFilterId] = useState<number>(1);

    useEffect(() => {
        const initComponent = async () => {
            let stationsResponse = await getAllStation(true, true);

            if (stationsResponse.status == Status.OK && stationsResponse.data != null) {
                console.log(stationsResponse.data)
                setStations(stationsResponse.data.stationList);
                console.log(stations)
            }

            await signalRService.startConnection();
            const cookie = await getToken();
            const token: string = cookie?.value || "";

            const monitoringReq: MonitoringRequest = {
                token: token
            }

            signalRService.invoke<MeasurePointDataDto>("InitiateMonitoring", monitoringReq)
            signalRService.on("receivedMeasurePointData", (newMeasurePoint: MeasurePointDataDto) => {
                setMeasurePoints((prevPoints) => [...prevPoints, newMeasurePoint]);
            });
        };

        initComponent();

        return () => {
            signalRService.stopConnection();
        };
    }, []);

    const filteredPoints = measurePoints.filter((point) => point.id === filterId);
    const defaultIcon = new Icon({
        className: "sm-map-marker",
        iconUrl: "icons/sm-marker.svg",
        iconSize: [25, 41],
        iconAnchor: [12, 41],
        popupAnchor: [1, -34],
    });

    const emittingIcon = new Icon({
        className: "sm-map-marker emitting",
        iconUrl: "icons/sm-marker.svg",
        iconSize: [25, 41],
        iconAnchor: [12, 41],
        popupAnchor: [1, -34],
    });

    return (
        <div>
            <h1>Points de Mesure</h1>
            <div style={{ marginBottom: "1rem" }}>
                <label htmlFor="filterId">Filtrer par ID (entre 1 et 50) : </label>
                <input
                    id="filterId"
                    type="number"
                    min="1"
                    max="50"
                    value={filterId}
                    onChange={(e) => setFilterId(Number(e.target.value))}
                    style={{ width: "50px" }}
                />
            </div>
            <LineChart width={600} height={300} data={filteredPoints}>
                <Line type="monotone" dataKey="value" stroke="#8884d8" />
                <CartesianGrid stroke="#ccc" />
                <XAxis dataKey="id" />
                <YAxis />
            </LineChart>

            <MapContainer center={[46.603354, 1.888334]} zoom={6} style={{ height: "500px", width: "100%" }}>
                <TileLayer
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                    attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                />

                {
                    stations.map((station) => (
                        <Marker
                            key={station.id}
                            position={[station.latitude, station.longitude]}
                            icon={emittingIcon}
                        >
                            <Popup>
                                <strong>{station.name}</strong>
                                <br />
                                Type : {station.type == StationType.Professionnal ? "Profsionnal" : "Private"}
                                <br />
                                ID : {station.id}
                            </Popup>
                        </Marker>
                    ))
                }
            </MapContainer>
        </div>
    );
}

export default socketConnector