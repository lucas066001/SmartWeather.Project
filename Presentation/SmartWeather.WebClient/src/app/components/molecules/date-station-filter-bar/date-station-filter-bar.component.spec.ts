import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DateStationFilterBarComponent } from './date-station-filter-bar.component';

describe('DateStationFilterBarComponent', () => {
  let component: DateStationFilterBarComponent;
  let fixture: ComponentFixture<DateStationFilterBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DateStationFilterBarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DateStationFilterBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
