import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CambiaPasswordComponent } from './cambia-password.component';

describe('CambiaPasswordComponent', () => {
  let component: CambiaPasswordComponent;
  let fixture: ComponentFixture<CambiaPasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CambiaPasswordComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CambiaPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
