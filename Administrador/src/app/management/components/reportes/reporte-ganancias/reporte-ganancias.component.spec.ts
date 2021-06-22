import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReporteGananciasComponent } from './reporte-ganancias.component';

describe('ReporteGananciasComponent', () => {
  let component: ReporteGananciasComponent;
  let fixture: ComponentFixture<ReporteGananciasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReporteGananciasComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReporteGananciasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
