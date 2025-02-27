import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WateringPageComponent } from './watering.component';

describe('WateringComponent', () => {
  let component: WateringPageComponent;
  let fixture: ComponentFixture<WateringPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WateringPageComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(WateringPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
