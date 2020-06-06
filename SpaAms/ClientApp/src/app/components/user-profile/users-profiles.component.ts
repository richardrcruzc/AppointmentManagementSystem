import { Component, Inject, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

import { User } from '../../shared/user';

import { ApiResult } from '../../base.service';
import { UserService } from './user.service';


@Component({
  selector: 'app-users',
  templateUrl: './users-profiles.component.html',
  styleUrls: ['./users-profiles.component.css']
})

export class UsersProfilesComponent {
  public displayedColumns: string[] = ['id',  'fullName', 'mobile', 'email', 'active', 'title', 'canBook'];
  public users: MatTableDataSource<User>;

  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;
  public defaultSortColumn: string = "fullName";
  public defaultSortOrder: string = "asc";

  defaultFilterColumn: string = "fullName";
  filterQuery: string = null;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private userService: UserService) {
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

    let sortColumn = (this.sort)
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

    this.userService.getData<ApiResult<User>>(
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
        this.users = new MatTableDataSource<User>(result.data);
      }, error => console.error(error));
  }
}
