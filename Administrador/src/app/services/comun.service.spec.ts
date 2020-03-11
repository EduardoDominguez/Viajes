import { TestBed } from '@angular/core/testing';

import { ComunService } from './comun.service';

describe('ComunService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ComunService = TestBed.get(ComunService);
    expect(service).toBeTruthy();
  });
});
