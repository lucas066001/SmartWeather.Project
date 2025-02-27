import { AfterViewInit, Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonMapCenterPoint, CommonPointRadius } from '@constants/ui/map';
import { LatLngExpression } from "leaflet";
import L from 'leaflet';

@Component({
  selector: 'app-map-points',
  imports: [],
  templateUrl: './map-points.component.html',
  styleUrl: './map-points.component.css'
})
export class MapPointsComponent implements AfterViewInit, OnChanges {


  ngOnChanges(changes: SimpleChanges): void {
    if (changes["pointsCoordinates"]) {
      setTimeout(() => {
        this.initMap();
      });
    }
  }


  @Input()
  set focusCoordinates(value: LatLngExpression | null) {
    this._focusCoordinates = value;
    if (this._focusCoordinates) {
      this.map?.panTo(this._focusCoordinates);
      this.map?.setZoom(12)
    }
  }
  get focusCoordinates(): LatLngExpression | null {
    return this._focusCoordinates;
  }

  @Input() pointsCoordinates: LatLngExpression[] = [];

  currentMapId: string = 'map-' + Math.random().toString(36).substring(7);
  _focusCoordinates: LatLngExpression | null = null;

  private map: L.Map | undefined;
  private radius: number = CommonPointRadius;

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
      center: CommonMapCenterPoint,
      zoom: 4
    });

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors'
    })?.addTo(this.map);

    this.pointsCoordinates.forEach((coords: LatLngExpression) => {
      if (!this.map) return;

      L.circle(coords, {
        radius: 10,
        color: 'var(--sm-tertiary-light-color)',
        fillColor: 'var(--sm-tertiary-light-color)',
        fillOpacity: 1
      }).addTo(this.map);

      L.circle(coords, {
        radius: this.radius,
        color: 'var(--sm-tertiary-light-color)',
        fillColor: 'var(--sm-tertiary-light-color)',
        fillOpacity: 0.2
      }).addTo(this.map);
    });
  }
}
