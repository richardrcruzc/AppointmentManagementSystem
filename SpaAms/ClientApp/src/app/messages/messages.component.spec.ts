import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { AngularMaterialModule } from '../angular-material.module';
import { of } from 'rxjs';

import { LocationsComponent } from './locations.component';
import { Location, LocationList } from './location';
import { LocationService } from './location.service';
import { ApiResult } from '../base.service';

describe('LocationsComponent', () => {
  let fixture: ComponentFixture<LocationsComponent>;
  let component: LocationsComponent;
  // async beforeEach(): testBed initialization
  beforeEach(async(() => {

    // Create a mock locatinService object with a mock 'getData' method
    let locationService = jasmine.createSpyObj<LocationService>('LocationService', ['getData']);
    // Configure the 'getData' spy method
    locationService.getData.and.returnValue(
      // return an Observable with some test data
      of<ApiResult<LocationList>>(<ApiResult<LocationList>>{
        data: [
          <LocationList>{
            id:1,
            description: 'TestCity1',
            contactName:'Contact name',
            city: 'City name',
            phone: '1234567890',
            totalMsgs: 1,
            totalAppt: 2,
            totalClosed: 3
          },
          <LocationList>{
            id: 2,
            description: 'TestCity2',
            contactName: 'Contact name',
            city: 'City name',
            phone: '1234567890',
            totalMsgs: 1,
            totalAppt: 2,
            totalClosed: 3
          },
          <LocationList>{
            id: 3,
            description: 'TestCity3',
            contactName: 'Contact name',
            city: 'City name',
            phone: '1234567890',
            totalMsgs: 1,
            totalAppt: 2,
            totalClosed: 3
          }
        ],
        totalCount: 3,
        pageIndex: 0,
        pageSize: 10
      }));

    

    TestBed.configureTestingModule({
      declarations: [LocationsComponent],
      imports: [
        BrowserAnimationsModule,
        AngularMaterialModule
      ],
      providers: [{
        provide: LocationService,
        useValue: locationService
      }

      ],
    })
      .compileComponents();
  }));

  // synchronous beforeEach(): fixtures and components setup
  beforeEach(() => {
    fixture = TestBed.createComponent(LocationsComponent);
    component = fixture.componentInstance;
    component.paginator = jasmine.createSpyObj(
      "MatPaginator", ["length", "pageIndex", "pageSize"]
    );
    fixture.detectChanges(); 

  });

  it('should display a "Locations" id, name, Contact Name, Total Mesg,Total Appt,Closed Dates', async(() => {
    let title = fixture.nativeElement
      .querySelector('h1');
    expect(title.textContent).toEqual('Locations');
  }));

  it('should contain a table with a list of one or more locations', async(() => {
    let table = fixture.nativeElement
      .querySelector('table.mat-table');
    let tableRows = table
      .querySelectorAll('tr.mat-row');
    expect(tableRows.length).toBeGreaterThan(0);
  }));

  
});


