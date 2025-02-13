import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfiguratorSelectorComponent } from './configurator-selector.component';

describe('ConfiguratorSelectorComponent', () => {
  let component: ConfiguratorSelectorComponent;
  let fixture: ComponentFixture<ConfiguratorSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfiguratorSelectorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfiguratorSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
