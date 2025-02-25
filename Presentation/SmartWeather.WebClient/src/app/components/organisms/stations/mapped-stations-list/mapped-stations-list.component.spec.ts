import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MappedStationsListComponent } from './mapped-stations-list.component';

describe('MappedStationsListComponent', () => {
  let component: MappedStationsListComponent;
  let fixture: ComponentFixture<MappedStationsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MappedStationsListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MappedStationsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
