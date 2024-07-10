import { TestBed } from '@angular/core/testing';

import { ConfirmLeaveService } from './confirm-leave.service';

describe('ConfirmLeaveService', () => {
  let service: ConfirmLeaveService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ConfirmLeaveService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
