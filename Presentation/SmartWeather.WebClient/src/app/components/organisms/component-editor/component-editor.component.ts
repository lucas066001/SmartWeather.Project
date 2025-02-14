import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MeasureUnitLabels } from '@constants/measure-unit';
import { ComponentResponse } from '@models/dtos/component-dtos';
import { ThemeService } from '@services/core/theme.service';

@Component({
  selector: 'app-component-editor',
  imports: [CommonModule],
  templateUrl: './component-editor.component.html',
  styleUrl: './component-editor.component.css'
})
export class ComponentEditorComponent {

  @Input() componentToEdit: ComponentResponse | null = null;
  @Input() isEditing: boolean = false;

  measureUnitLabels = MeasureUnitLabels;

  constructor(public themeService: ThemeService) { }

}
