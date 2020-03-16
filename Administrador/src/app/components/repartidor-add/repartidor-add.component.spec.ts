import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RepartidorAddComponent } from './repartidor-add.component';

describe('RepartidorAddComponent', () => {
  let component: RepartidorAddComponent;
  let fixture: ComponentFixture<RepartidorAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RepartidorAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RepartidorAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
