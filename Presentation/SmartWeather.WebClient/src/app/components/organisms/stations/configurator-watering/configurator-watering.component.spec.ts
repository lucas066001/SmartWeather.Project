import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfiguratorWateringComponent } from './configurator-watering.component';

describe('ConfiguratorWateringComponent', () => {
  let component: ConfiguratorWateringComponent;
  let fixture: ComponentFixture<ConfiguratorWateringComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfiguratorWateringComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfiguratorWateringComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
