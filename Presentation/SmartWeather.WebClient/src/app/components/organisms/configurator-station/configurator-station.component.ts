import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { StationResponse, StationUpdateRequest } from '@models/dtos/station-dtos';
import { ThemeService } from '@services/core/theme.service';
import { ComponentEditorComponent } from '../component-editor/component-editor.component';
import { ComponentService } from '@services/component/component.service';
import { Status } from '@constants/api/api-status';
import { ComponentResponse } from '@models/dtos/component-dtos';
import { MapEditorComponent } from '../map-editor/map-editor.component';
import { StationService } from '@services/station/station.service';
import { AuthService } from '@services/core/auth.service';
import { ApiResponse } from '@models/api-response';

@Component({
  selector: 'app-configurator-station',
  imports: [CommonModule, ComponentEditorComponent, MapEditorComponent, ReactiveFormsModule],
  templateUrl: './configurator-station.component.html',
  styleUrl: './configurator-station.component.css'
})
export class ConfiguratorStationComponent {

  stationForm: FormGroup = new FormGroup({
    name: new FormControl(''),
    latitude: new FormControl(0),
    longitude: new FormControl(0)
  });
  components: ComponentResponse[] | null = null;
  isEditing: boolean = false;

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

      this.stationForm.setValue({
        name: this._stationToEdit.name || '',
        latitude: this._stationToEdit.latitude || 0,
        longitude: this._stationToEdit.longitude || 0
      });

      this.componentService.getFromStation(this._stationToEdit.id, true).subscribe({
        next: (response) => {
          console.log(response)
          if (response.status == Status.OK && response.data?.componentList) {
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

  updateCoordinates(obj: any) {
    console.log("corrdinate update");
    console.log(obj);
    this.stationForm.patchValue({ latitude: obj.lat, longitude: obj.lng });
  }
}
