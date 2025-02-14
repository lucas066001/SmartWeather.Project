import { CommonModule } from '@angular/common';
import { Component, Input, AfterViewInit, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { ThemeService } from '@services/core/theme.service';
import * as L from 'leaflet';

@Component({
  selector: 'app-map-editor',
  imports: [CommonModule],
  templateUrl: './map-editor.component.html',
  styleUrl: './map-editor.component.css'
})
export class MapEditorComponent implements AfterViewInit, OnChanges {
  @Input() latitude: number = 48.8566;
  @Input() longitude: number = 2.3522;
  @Input() isEditing: boolean = false;

  @Output() coordinateChange = new EventEmitter<{ lat: number, lng: number }>();

  currentMapId: string = 'map-' + Math.random().toString(36).substring(7);

  private map: L.Map | undefined;
  private marker: L.Circle | undefined;
  private circle: L.Circle | undefined;
  private radius: number = 1000; // Rayon par défaut en mètres

  constructor(public themeService: ThemeService) { }

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
      radius: 10,
      color: 'var(--sm-tertiary-light-color)',
      fillColor: 'var(--sm-tertiary-light-color)',
      fillOpacity: 1
    }).addTo(this.map);

    // Ajouter un cercle de rayon autour du point
    this.circle = L.circle([this.latitude, this.longitude], {
      radius: this.radius,
      color: 'var(--sm-tertiary-light-color)',
      fillColor: 'var(--sm-tertiary-light-color)',
      fillOpacity: 0.2
    }).addTo(this.map);

    // Ajoute l'écouteur d'événements pour détecter les clics sur la carte
    this.map.on('click', (event: L.LeafletMouseEvent) => {
      if (this.isEditing) {
        this.updateMarkerPosition(event.latlng.lat, event.latlng.lng);
        this.coordinateChange.emit({ lat: event.latlng.lat, lng: event.latlng.lng });
      }
    });
  }

  private updateMarkerPosition(lat: number, lng: number): void {
    this.latitude = lat;
    this.longitude = lng;

    if (this.marker && this.circle) {
      this.marker.setLatLng([lat, lng]);
      this.circle.setLatLng([lat, lng]);
    }

    if (this.map) {
      this.map.setView([lat, lng], this.map.getZoom());
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.map && this.marker && this.circle) {
      this.updateMarkerPosition(this.latitude, this.longitude);
    }
  }
}
