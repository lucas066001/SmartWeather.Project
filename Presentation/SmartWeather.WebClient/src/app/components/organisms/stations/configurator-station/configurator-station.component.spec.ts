import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfiguratorStationComponent } from './configurator-station.component';

describe('ConfiguratorStationComponent', () => {
  let component: ConfiguratorStationComponent;
  let fixture: ComponentFixture<ConfiguratorStationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfiguratorStationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfiguratorStationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
