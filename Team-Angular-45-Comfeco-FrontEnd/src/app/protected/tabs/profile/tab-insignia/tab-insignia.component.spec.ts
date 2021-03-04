import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TabInsigniaComponent } from './tab-insignia.component';

describe('TabInsigniaComponent', () => {
  let component: TabInsigniaComponent;
  let fixture: ComponentFixture<TabInsigniaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TabInsigniaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TabInsigniaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
