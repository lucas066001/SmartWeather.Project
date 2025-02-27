import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterCanIconComponent } from './water-can-icon.component';

describe('WaterCanIconComponent', () => {
  let component: WaterCanIconComponent;
  let fixture: ComponentFixture<WaterCanIconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WaterCanIconComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WaterCanIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
