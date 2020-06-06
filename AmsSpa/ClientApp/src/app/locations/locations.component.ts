import { Component, Inject, ViewChild } from '@angular/core'; 
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

import { LocationList } from './location';

import { LocationService } from './location.service';
import { ApiResult } from '../base.service';


@Component({
  selector: 'app-locations',
  templateUrl: './locations.component.html',
  styleUrls: ['./locations.component.css']
})

export class LocationsComponent {
  public displayedColumns: string[] = ['id', 'description', 'contactName', 'phone','city', 'totalMsgs', 'totalAppt','totalClosed'];
  public locations: MatTableDataSource<LocationList>;

  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;
  public defaultSortColumn: string = "description";
  public defaultSortOrder: string = "asc";

  defaultFilterColumn: string = "description";
  filterQuery: string = null;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
  private locationService: LocationService) {
  }

  ngOnInit() {
    this.loadData(null);
  }

  loadData(query: string = null) {
    var pageEvent = new PageEvent();
    pageEvent.pageIndex = this.defaultPageIndex;
    pageEvent.pageSize = this.defaultPageSize;
    if (query) {
      this.filterQuery = query;
    }
    this.getData(pageEvent);
  }

  getData(event: PageEvent) {

    let sortColumn = (this.sort)
      ? this.sort.active
      : this.defaultSortColumn;

    let sortOrder = (this.sort)
      ? this.sort.direction
      : this.defaultSortOrder;

    let filterColumn = (this.filterQuery)
      ? this.defaultFilterColumn
      : null;

    let filterQuery = (this.filterQuery)
      ? this.filterQuery
      : null;

    this.locationService.getData<ApiResult<LocationList>>(
      event.pageIndex,
      event.pageSize,
      sortColumn,
      sortOrder,
      filterColumn,
      filterQuery)
      .subscribe(result => {
        this.paginator.length = result.totalCount;
        this.paginator.pageIndex = result.pageIndex;
        this.paginator.pageSize = result.pageSize;
        this.locations = new MatTableDataSource<LocationList>(result.data);
      }, error => console.error(error));
  }
}
