// "use client";

// import React, { useEffect } from "react";
// import dynamic from "next/dynamic";
// import { ISocketHandler } from "@/components/ui/socket/ISocketHandler";
// import { StationType } from "@/services/station/dtos/station_response";
// import "leaflet/dist/leaflet.css";
// import { defaultIcon } from "@/components/ui/maps/icons/defaultIcon";

// // Dynamically import Leaflet components to ensure client-only rendering
// const MapContainer = dynamic(() => import("react-leaflet").then((mod) => mod.MapContainer), { ssr: false });
// const TileLayer = dynamic(() => import("react-leaflet").then((mod) => mod.TileLayer), { ssr: false });
// const Marker = dynamic(() => import("react-leaflet").then((mod) => mod.Marker), { ssr: false });
// const Popup = dynamic(() => import("react-leaflet").then((mod) => mod.Popup), { ssr: false });


// function StationsMapEmitting({ dataPoints, stations }: ISocketHandler) {
//     useEffect(() => {
//         return () => {
//         };
//     }, []);
//     return (
//         <div id="map">
//             <MapContainer key={`${stations.length}-${dataPoints.length}`} center={[46.603354, 1.888334]} zoom={6} style={{ height: "500px", width: "100%" }}>
//                 <TileLayer
//                     url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
//                     attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
//                 />
//                 {stations.map((station) => (
//                     <Marker
//                         key={station.id}
//                         position={[station.latitude, station.longitude]}
//                         icon={defaultIcon}
//                     >
//                         <Popup>
//                             <strong>{station.name}</strong>
//                             <br />
//                             Type: {station.type === StationType.Professionnal ? "Professional" : "Private"}
//                             <br />
//                             ID: {station.id}
//                         </Popup>
//                     </Marker>
//                 ))}
//             </MapContainer>
//         </div>
//     );
// }

// export default StationsMapEmitting;
