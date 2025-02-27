import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ComponentEditorComponent } from '@components/organisms/components/component-editor/component-editor.component';
import { MapEditorComponent } from '../map-editor/map-editor.component';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ActuatorCommandRequest, ComponentResponse } from '@models/dtos/component-dtos';
import { StationResponse, StationUpdateRequest } from '@models/dtos/station-dtos';
import { ThemeService } from '@services/core/theme.service';
import { ComponentService } from '@services/component/component.service';
import { StationService } from '@services/station/station.service';
import { AuthService } from '@services/core/auth.service';
import { Status } from '@constants/api/api-status';
import { ApiResponse } from '@models/api-response';
import { MapPointsComponent } from '@components/atoms/map-points/map-points.component';
import { LatLngExpression } from 'leaflet';
import { ComponentType } from '@constants/entities/component-type';
import { ButtonComponent } from '@components/atoms/button/button.component';

@Component({
  selector: 'app-configurator-watering',
  imports: [CommonModule, ComponentEditorComponent, MapPointsComponent, ReactiveFormsModule, ButtonComponent],
  templateUrl: './configurator-watering.component.html',
  styleUrl: './configurator-watering.component.css'
})
export class ConfiguratorWateringComponent {

  stationForm: FormGroup = new FormGroup({
    name: new FormControl(''),
    latitude: new FormControl(0),
    longitude: new FormControl(0)
  });
  components: ComponentResponse[] | null = null;
  isEditing: boolean = false;
  stationsCoordinates: LatLngExpression[] = [];
  currentSelectedCoordinates: LatLngExpression | null = null;

  private _stationToEdit: StationResponse | null = null;

  @Output() stationUpdatedEvent: EventEmitter<StationResponse> = new EventEmitter<StationResponse>();

  @Input()
  set stationToEdit(value: StationResponse | null) {
    this._stationToEdit = value;
    this.onStationToEditChange();
  }

  get stationToEdit(): StationResponse | null {
    return this._stationToEdit;
  }

  constructor(public themeService: ThemeService, private componentService: ComponentService, private stationService: StationService, private authService: AuthService) { }

  onStationToEditChange() {
    this.setEditMode(false);
    if (this._stationToEdit) {

      this.stationsCoordinates = [[this._stationToEdit.latitude, this._stationToEdit.longitude]];
      this.currentSelectedCoordinates = [this._stationToEdit.latitude, this._stationToEdit.longitude];

      this.stationForm.setValue({
        name: this._stationToEdit.name || '',
        latitude: this._stationToEdit.latitude || 0,
        longitude: this._stationToEdit.longitude || 0
      });

      this.componentService.getFromStation(this._stationToEdit.id, true).subscribe({
        next: (response) => {
          console.log(response)
          if (response.status == Status.OK && response.data?.componentList) {
            // this.components = response.data.componentList.filter(c => c.type == ComponentType.Actuator);
            this.components = response.data.componentList;
          }
        },
        error: (error) => {
          console.error('Error while retrieving components:', error);
        }
      });
    }
  }

  setEditMode(isEditing: boolean) {
    this.isEditing = isEditing;
  }

  saveStation() {
    let userId = this.authService.getUserId();
    if (this.stationForm.valid && userId && this._stationToEdit) {
      const updatedStation: StationUpdateRequest = {
        id: this._stationToEdit.id,
        name: this.stationForm.value.name,
        latitude: this.stationForm.value.latitude,
        longitude: this.stationForm.value.longitude,
        userId: userId
      };

      this.stationService.update(updatedStation).subscribe(({
        next: (response) => {
          console.log('Update réussie :', response);
          if (response.status == Status.OK && response.data) {
            this.stationToEdit = response.data;
            this.stationUpdatedEvent.emit(response.data);
          }
        },
        error: (error: ApiResponse<StationResponse>) => {
          console.log(error);
        }
      }));

      this.setEditMode(false);
    }
  }

  handleWaterRequest(gpioPin: number) {
    const inputElement = document.getElementById("pinvalue-" + gpioPin) as HTMLInputElement;

    if (inputElement && this.stationToEdit) {
      const pinValue = inputElement.valueAsNumber;
      console.log("Valeur de l'input :", pinValue);
      let request: ActuatorCommandRequest = {
        componentGpioPin: gpioPin,
        componentValueUpdate: pinValue,
        stationId: this.stationToEdit?.id
      }
      this.componentService.actuatorCommand(request).subscribe(({
        next: (response) => {
          console.log(response);
        },
        error: (error) => {
          console.log(error);
        }
      }))
    }
  }

}
