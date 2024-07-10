import { TestBed } from '@angular/core/testing';

import { HandleApiErrorService } from './handle-api-error.service';

describe('HandleApiErrorService', () => {
  let service: HandleApiErrorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HandleApiErrorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
