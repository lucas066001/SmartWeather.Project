import { Icon } from "leaflet";

export const defaultIcon = new Icon({
    className: "sm-map-marker",
    iconUrl: "icons/sm-marker.svg",
    iconSize: [25, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34],
});