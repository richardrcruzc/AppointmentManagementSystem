import { Component, Inject, ViewChild } from '@angular/core'; 
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
 
import { ApiResult } from '../base.service';  
import { ClientI } from './client';
import { ClientService } from './client.service';


@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  //styleUrls: ['./clients.component.css']
})

export class ClientsComponent {
  public displayedColumns: string[] = ['id', 'fullName',  'mobile', 'email', 'active', 'sendNotificationBy', 'acceptsMarketingNotifications', 'photo'];
  public clients: MatTableDataSource<ClientI>;

  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;
  public defaultSortColumn: string = "LastName";
  public defaultSortOrder: string = "asc";

  defaultFilterColumn: string = "LastName";
  filterQuery: string = null;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
  private clientService: ClientService) {
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

    this.clientService.getData<ApiResult<ClientI>>(
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
        this.clients = new MatTableDataSource<ClientI>(result.data);
      }, error => console.error(error));
  }
}
