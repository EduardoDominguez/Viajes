import { TestBed } from '@angular/core/testing';

import { NodosService } from './nodos.service';

describe('NodosService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: NodosService = TestBed.get(NodosService);
    expect(service).toBeTruthy();
  });
});
