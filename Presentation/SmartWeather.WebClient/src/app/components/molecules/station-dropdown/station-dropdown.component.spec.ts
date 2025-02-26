import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StationDropdownComponent } from './station-dropdown.component';

describe('StationDropdownComponent', () => {
  let component: StationDropdownComponent;
  let fixture: ComponentFixture<StationDropdownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StationDropdownComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StationDropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
