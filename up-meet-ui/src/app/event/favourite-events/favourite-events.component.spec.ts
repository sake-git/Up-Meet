import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavouriteEventsComponent } from './favourite-events.component';

describe('FavouriteEventsComponent', () => {
  let component: FavouriteEventsComponent;
  let fixture: ComponentFixture<FavouriteEventsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FavouriteEventsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FavouriteEventsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
