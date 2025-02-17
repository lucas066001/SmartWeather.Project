import { TestBed } from '@angular/core/testing';

import { MeasurePointService } from './measure-point.service';

describe('MeasurePointService', () => {
  let service: MeasurePointService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MeasurePointService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
