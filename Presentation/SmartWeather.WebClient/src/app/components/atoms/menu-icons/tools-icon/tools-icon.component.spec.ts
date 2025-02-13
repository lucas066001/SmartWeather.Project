import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ToolsIconComponent } from './tools-icon.component';

describe('ToolsIconComponent', () => {
  let component: ToolsIconComponent;
  let fixture: ComponentFixture<ToolsIconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ToolsIconComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ToolsIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
