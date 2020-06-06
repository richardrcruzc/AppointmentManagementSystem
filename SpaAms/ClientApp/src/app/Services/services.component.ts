import { Component, Inject, ViewChild } from '@angular/core'; 
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
 
import { ApiResult } from '../base.service'; 
import { ServiceService } from './service.service';
import { Service } from './service';


@Component({
  selector: 'app-services',
  templateUrl: './services.component.html',
  //styleUrls: ['./services.component.css']
})

export class ServicesComponent {
  public displayedColumns: string[] = ['id', 'serviceName', 'durationHour', 'durationMinute', 'price', 'photo', 'serviceDescription', 'activeStatus', 'serviceCategoryId'];
  public services: MatTableDataSource<Service>;

  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;
  public defaultSortColumn: string = "serviceName";
  public defaultSortOrder: string = "asc";

  defaultFilterColumn: string = "serviceName";
  filterQuery: string = null;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
  private serviceService: ServiceService) {
  }

  ngOnInit() {
    this.loadData(null);
  }

  loadData(query: string = null) {
    let pageEvent = new PageEvent();
    pageEvent.pageIndex = this.defaultPageIndex;
    pageEvent.pageSize = this.defaultPageSize;
    if (query) {
      this.filterQuery = query;
    }
    this.getData(pageEvent);
  }

  getData(event: PageEvent) {

    let  sortColumn = (this.sort)
      ? this.sort.active
      : this.defaultSortColumn;

    if (sortColumn === undefined) {
      sortColumn = "";
    }
    let sortOrder = (this.sort)
      ? this.sort.direction
      : this.defaultSortOrder;

    let filterColumn = (this.filterQuery)
      ? this.defaultFilterColumn
      : null;

    let filterQuery = (this.filterQuery)
      ? this.filterQuery
      : null;

    this.serviceService.getData<ApiResult<Service>>(
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
        this.services = new MatTableDataSource<Service>(result.data);
      }, error => console.error(error));
  }
}
