import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TabEventComponent } from './tab-event.component';

describe('TabEventComponent', () => {
  let component: TabEventComponent;
  let fixture: ComponentFixture<TabEventComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TabEventComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TabEventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
