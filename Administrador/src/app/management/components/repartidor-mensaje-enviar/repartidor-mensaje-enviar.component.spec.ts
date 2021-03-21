import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RepartidorMensajeEnviarComponent } from './repartidor-mensaje-enviar.component';

describe('RepartidorMensajeEnviarComponent', () => {
  let component: RepartidorMensajeEnviarComponent;
  let fixture: ComponentFixture<RepartidorMensajeEnviarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RepartidorMensajeEnviarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RepartidorMensajeEnviarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
