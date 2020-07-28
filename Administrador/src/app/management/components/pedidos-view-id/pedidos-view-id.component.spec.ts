import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PedidosViewIdComponent } from './pedidos-view-id.component';

describe('PedidosViewIdComponent', () => {
  let component: PedidosViewIdComponent;
  let fixture: ComponentFixture<PedidosViewIdComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PedidosViewIdComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PedidosViewIdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
