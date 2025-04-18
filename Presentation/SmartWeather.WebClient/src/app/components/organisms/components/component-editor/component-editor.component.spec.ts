import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentEditorComponent } from './component-editor.component';

describe('ComponentEditorComponent', () => {
  let component: ComponentEditorComponent;
  let fixture: ComponentFixture<ComponentEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComponentEditorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComponentEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
