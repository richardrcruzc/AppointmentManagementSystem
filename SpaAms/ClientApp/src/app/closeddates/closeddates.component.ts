import { Component, Inject, ViewChild } from '@angular/core'; 
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
 
import { ApiResult } from '../base.service';
import { ClosedDate } from './closeddate';
import { ClosedDatesService } from './closeddate.service';
 

@Component({
  selector: 'app-closeddates',
  templateUrl: './closeddates.component.html',
  styleUrls: ['./closeddates.component.css']
})

export class ClosedDatesComponent {
  public displayedColumns: string[] = ['id', 'from', 'to', 'description',   'location'];
  public closeddates: MatTableDataSource<ClosedDate>;

  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;
  public defaultSortColumn: string = "from";
  public defaultSortOrder: string = "asc";

  defaultFilterColumn: string = "from";
  filterQuery: string = null;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private closedateService: ClosedDatesService) {
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

    this.closedateService.getData<ApiResult<ClosedDate>>(
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
        this.closeddates = new MatTableDataSource<ClosedDate>(result.data);
      }, error => console.error(error));
  }
}
