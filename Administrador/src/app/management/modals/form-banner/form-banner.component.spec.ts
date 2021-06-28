import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormBannerComponent } from './form-banner.component';

describe('FormBannerComponent', () => {
  let component: FormBannerComponent;
  let fixture: ComponentFixture<FormBannerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FormBannerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FormBannerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
