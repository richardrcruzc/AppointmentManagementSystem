import { Component, Inject, ViewChild } from '@angular/core'; 
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
 
import { ApiResult } from '../base.service';
import { Category } from './category';
import { CategoryService } from './category.service';


@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  //styleUrls: ['./categories.component.css']
})

export class CategoriesComponent {
  public displayedColumns: string[] = ['id', 'description'];
  public categories: MatTableDataSource<Category>;

  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;
  public defaultSortColumn: string = "description";
  public defaultSortOrder: string = "asc";

  defaultFilterColumn: string = "description";
  filterQuery: string = null;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
  private categoryService: CategoryService) {
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

    this.categoryService.getData<ApiResult<Category>>(
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
        this.categories = new MatTableDataSource<Category>(result.data);
      }, error => console.error(error));
  }
}
