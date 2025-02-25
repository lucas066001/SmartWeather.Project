import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Status } from '@constants/api/api-status';
import { MeasureUnitLabels } from '@constants/entities/measure-unit';
import { PinoutLabels } from '@constants/ui/pinout-mapping';
import { ApiResponse } from '@models/api-response';
import { ComponentResponse, ComponentUpdateRequest } from '@models/dtos/component-dtos';
import { MeasurePointResponse, MeasurePointUpdateRequest } from '@models/dtos/measurepoint-dtos';
import { ComponentService } from '@services/component/component.service';
import { ThemeService } from '@services/core/theme.service';
import { MeasurePointService } from '@services/measure-point/measure-point.service';

@Component({
  selector: 'app-component-editor',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './component-editor.component.html',
  styleUrl: './component-editor.component.css'
})
export class ComponentEditorComponent {

  @Input() componentToEdit: ComponentResponse | null = null;

  isEditing: boolean = false;
  isErrorSaving: boolean = false;
  isSubmitting: boolean = false;

  unitKeys = Array.from(MeasureUnitLabels.keys());
  pinKeys = Array.from(PinoutLabels.keys());
  measureUnitLabels = MeasureUnitLabels;
  pinoutLabels = PinoutLabels;

  componentEditForm: FormGroup;

  get measurePoints(): FormArray {
    return this.componentEditForm.get('measurePoints') as FormArray;
  }

  constructor(public themeService: ThemeService, private fb: FormBuilder, private componentService: ComponentService, private measurePointService: MeasurePointService) {
    this.componentEditForm = new FormGroup({
      componentName: new FormControl(''),
      componentPin: new FormControl(''),
      measurePoints: this.fb.array([])
    });
  }

  setEditMode(isEditing: boolean) {
    console.log(this.componentToEdit)

    this.isEditing = isEditing;

    if (this.componentToEdit && this.componentToEdit.measurePoints) {
      this.componentEditForm.patchValue({ componentName: this.componentToEdit.name, componentPin: this.componentToEdit.gpioPin });

      this.measurePoints.clear(); // Réinitialise le FormArray

      this.componentToEdit.measurePoints.forEach(mp => {
        this.measurePoints.push(this.createMeasurePointForm(mp));
      });
      console.log(this.componentEditForm)
      console.log(this.fb)
      console.log(Array.from(MeasureUnitLabels.keys()))
    }
  }

  createMeasurePointForm(measurePoint?: MeasurePointResponse): FormGroup {
    return this.fb.group({
      id: [measurePoint?.id || 0],
      color: [measurePoint?.color || '#000000'],
      name: [measurePoint?.name || ''],
      unit: [measurePoint?.unit || '']
    });
  }

  saveComponent() {
    if (this.componentEditForm.invalid || this.componentToEdit == null) {
      this.isErrorSaving = true;
      return;
    }

    this.isSubmitting = true;

    let updateComponentResquest: ComponentUpdateRequest = {
      id: this.componentToEdit.id,
      gpioPin: this.componentEditForm.value.componentPin,
      name: this.componentEditForm.value.componentName,
      color: "#000000",
      type: this.componentToEdit.type,
      stationId: this.componentToEdit.stationId,
    }

    this.componentService.update(updateComponentResquest).subscribe(({
      next: (response) => {
        console.log('Update réussi :', response);
        if (response.status == Status.OK && response.data) {
          this.componentToEdit = response.data;
        }
        this.isSubmitting = false;
      },
      error: (error: ApiResponse<ComponentResponse>) => {
        console.log('Erreur de connexion :', error);
        this.isErrorSaving = true;
        this.isSubmitting = false;
      }
    }));

    this.componentEditForm.value.measurePoints.forEach((mpToUpdate: MeasurePointUpdateRequest) => {
      console.log(mpToUpdate);
      let measurePointUpdateRequest: MeasurePointUpdateRequest = {
        id: mpToUpdate.id,
        name: mpToUpdate.name,
        color: mpToUpdate.color,
        unit: mpToUpdate.unit,
        componentId: this.componentToEdit?.id || 0,
      }

      this.measurePointService.update(measurePointUpdateRequest).subscribe(({
        next: (response) => {
          console.log('Update réussi :', response);
          if (response.status == Status.OK && response.data && this.componentToEdit) {
            let foundMeasurePoint = this.componentToEdit.measurePoints?.findIndex(m => m.id == response.data?.id);
            if (foundMeasurePoint && foundMeasurePoint >= 0 && this.componentToEdit.measurePoints) {
              this.componentToEdit.measurePoints[foundMeasurePoint] = response.data;
            }
          }
          this.isSubmitting = false;
        },
        error: (error: ApiResponse<MeasurePointResponse>) => {
          console.log('Erreur de connexion :', error);
          this.isErrorSaving = true;
          this.isSubmitting = false;
        }
      }));


    });

  }

  resetError() {
    this.isErrorSaving = false;
  }
}
