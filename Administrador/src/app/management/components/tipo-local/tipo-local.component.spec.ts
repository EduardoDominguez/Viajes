import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TipoLocalComponent } from './tipo-local.component';

describe('TipoLocalComponent', () => {
  let component: TipoLocalComponent;
  let fixture: ComponentFixture<TipoLocalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TipoLocalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TipoLocalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
