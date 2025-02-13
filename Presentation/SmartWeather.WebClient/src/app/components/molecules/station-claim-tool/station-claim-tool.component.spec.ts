import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StationClaimToolComponent } from './station-claim-tool.component';

describe('StationClaimToolComponent', () => {
  let component: StationClaimToolComponent;
  let fixture: ComponentFixture<StationClaimToolComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StationClaimToolComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StationClaimToolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
