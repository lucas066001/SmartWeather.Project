"use client"
import { getToken } from '@/lib/tokenManager';
import { MeasurePointDataDto, MonitoringRequest } from '@/services/dtos/socket';
import { signalRService } from '@/services/socket_service';
import React, { useEffect, useState } from 'react'

function socketConnector() {
    const [messages, setMessages] = useState<string[]>([]);
    const [input, setInput] = useState("");

    useEffect(() => {
        const startSignalRConnection = async () => {
            await signalRService.startConnection();
            const cookie = await getToken();
            const token: string = cookie?.value || "";

            const monitoringReq: MonitoringRequest = {
                token: token
            }

            signalRService.invoke<MeasurePointDataDto>("InitiateMonitoring", monitoringReq)
            signalRService.on("receivedMeasurePointData", (mp: MeasurePointDataDto) => {
                // setMessages((prev) => [...prev, `${user}: ${message}`]);
                console.log(mp)
            });
        };

        startSignalRConnection();

        return () => {
            signalRService.stopConnection();
        };
    }, []);
    return (
        <div>socketConnector</div>
    )
}

export default socketConnector