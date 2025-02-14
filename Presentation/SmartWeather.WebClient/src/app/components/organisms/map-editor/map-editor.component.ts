import { Component, Input, AfterViewInit } from '@angular/core';
import * as L from 'leaflet';

@Component({
  selector: 'app-map-editor',
  imports: [],
  templateUrl: './map-editor.component.html',
  styleUrl: './map-editor.component.css'
})
export class MapEditorComponent implements AfterViewInit {
  @Input() latitude: number = 48.8566;
  @Input() longitude: number = 2.3522;

  currentMapId: string = 'map-' + Math.random().toString(36).substring(7);

  private map: L.Map | undefined;
  private marker: L.Circle | undefined;
  private circle: L.Circle | undefined;
  private radius: number = 1000; // Rayon par défaut en mètres

  constructor() { }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.initMap();

    }, 100);
  }

  private initMap(): void {
    if (this.map) {
      this.map.remove();
    }

    this.map = L.map(this.currentMapId, {
      center: [this.latitude, this.longitude],
      zoom: 13
    });

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors'
    }).addTo(this.map);

    // Ajouter un point simple au lieu d'un marqueur classique
    this.marker = L.circle([this.latitude, this.longitude], {
      radius: 10, // Petit cercle représentant le point
      color: 'var(--sm-tertiary-light-color)', // Couleur de la bordure
      fillColor: 'var(--sm-tertiary-light-color)', // Couleur de remplissage
      fillOpacity: 1
    }).addTo(this.map);

    // Ajouter un cercle de rayon autour du point
    this.circle = L.circle([this.latitude, this.longitude], {
      radius: this.radius, // Rayon dynamique en mètres
      color: 'var(--sm-tertiary-light-color)',
      fillColor: 'var(--sm-tertiary-light-color)',
      fillOpacity: 0.2 // Opacité plus faible
    }).addTo(this.map);
  }

  // Met à jour la position du marqueur si les inputs changent
  ngOnChanges(): void {
    if (this.map && this.marker) {
      this.marker.setLatLng([this.latitude, this.longitude]);
      this.map.setView([this.latitude, this.longitude], 13);
    }
  }
}
