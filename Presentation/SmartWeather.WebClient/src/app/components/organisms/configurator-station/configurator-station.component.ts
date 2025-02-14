import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { StationResponse } from '@models/dtos/station-dtos';
import { ThemeService } from '@services/core/theme.service';
import { ComponentEditorComponent } from '../component-editor/component-editor.component';
import { ComponentService } from '@services/component/component.service';
import { Status } from '@constants/api-status';
import { ComponentResponse } from '@models/dtos/component-dtos';
import { MapEditorComponent } from '../map-editor/map-editor.component';

@Component({
  selector: 'app-configurator-station',
  imports: [CommonModule, ComponentEditorComponent, MapEditorComponent],
  templateUrl: './configurator-station.component.html',
  styleUrl: './configurator-station.component.css'
})
export class ConfiguratorStationComponent {

  private _stationToEdit: StationResponse | null = null;
  components: ComponentResponse[] | null = null;

  @Input()
  set stationToEdit(value: StationResponse | null) {
    this._stationToEdit = value;
    this.onStationToEditChange();
  }

  get stationToEdit(): StationResponse | null {
    return this._stationToEdit;
  }

  constructor(public themeService: ThemeService, public componentService: ComponentService) { }

  onStationToEditChange() {
    if (this._stationToEdit) {
      this.componentService.getFromStation(this._stationToEdit.id, true).subscribe({
        next: (response) => {
          console.log('Components retreived :', response);
          if (response.status == Status.OK && response.data?.componentList && response.data.componentList.length > 0) {
            this.components = response.data.componentList;
            console.log(this.components);
          }
        },
        error: (error) => {
          console.error('Error while retreiving stations :', error);
        }
      });
    }
  }

}
