import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserQuickAccessComponent } from './user-quick-access.component';

describe('UserQuickAccessComponent', () => {
  let component: UserQuickAccessComponent;
  let fixture: ComponentFixture<UserQuickAccessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserQuickAccessComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserQuickAccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
