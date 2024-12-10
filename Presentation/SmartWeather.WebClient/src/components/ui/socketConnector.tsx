"use client"
import { getToken } from '@/lib/tokenManager';
import { MeasurePointDataDto, MonitoringRequest } from '@/services/dtos/socket';
import { signalRService } from '@/services/socket_service';
import React, { useEffect, useState } from 'react'
import { LineChart, Line, CartesianGrid, XAxis, YAxis } from 'recharts';

function socketConnector() {
    const [measurePoints, setMeasurePoints] = useState<MeasurePointDataDto[]>([]);
    const [filterId, setFilterId] = useState<number>(1);

    useEffect(() => {
        const startSignalRConnection = async () => {
            await signalRService.startConnection();
            const cookie = await getToken();
            const token: string = cookie?.value || "";

            const monitoringReq: MonitoringRequest = {
                token: token
            }

            signalRService.invoke<MeasurePointDataDto>("InitiateMonitoring", monitoringReq)
            signalRService.on("receivedMeasurePointData", (newMeasurePoint: MeasurePointDataDto) => {
                // if (newMeasurePoint.id == filterId) {
                setMeasurePoints((prevPoints) => [...prevPoints, newMeasurePoint]);
                // }
            });
        };

        startSignalRConnection();

        return () => {
            signalRService.stopConnection();
        };
    }, []);

    const filteredPoints = measurePoints.filter((point) => point.id === filterId);


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
        </div>
    );
}

export default socketConnector