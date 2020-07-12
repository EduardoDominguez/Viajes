import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductoExtrasModalAeComponent } from './producto-extras-modal-ae.component';

describe('ProductoExtrasModalAeComponent', () => {
  let component: ProductoExtrasModalAeComponent;
  let fixture: ComponentFixture<ProductoExtrasModalAeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductoExtrasModalAeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductoExtrasModalAeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
