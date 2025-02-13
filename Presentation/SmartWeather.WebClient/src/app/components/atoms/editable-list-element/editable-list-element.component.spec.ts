import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditableListElementComponent } from './editable-list-element.component';

describe('EditableListElementComponent', () => {
  let component: EditableListElementComponent;
  let fixture: ComponentFixture<EditableListElementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditableListElementComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditableListElementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
