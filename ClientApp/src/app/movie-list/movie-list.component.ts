import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { catchError, debounceTime, distinctUntilChanged, map, Observable, startWith, switchMap } from 'rxjs';
import { MovieService } from '../service/movie.service';
import { MatInputModule } from "@angular/material/input";
import { MatPaginator, MatPaginatorModule } from "@angular/material/paginator";
import { MatSortModule } from "@angular/material/sort";
import { MatTableDataSource, MatTableModule } from "@angular/material/table";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MovieModel, MoviePageModel } from '../models/movie-model';
import { FormBuilder } from '@angular/forms';


@Component({
  selector: 'app-movie-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, MatInputModule, MatPaginatorModule, MatProgressSpinnerModule,
    MatSortModule, MatTableModule],
  templateUrl: './movie-list.component.html',
  styleUrls: ['./movie-list.component.scss']
})
export class MovieListComponent implements OnInit {
  searchForm = this.formBuilder.group({
    nameFilter: new FormControl(''),
    genreFilter: new FormControl('All')
  });

  displayedColumns: string[] = [
    'Title',
    'Release_Date',
    'Overview',
    'Popularity',
    'Genre',
    'PosterUrl',
  ];
  private searchStr: string = '';
  public totalData: number = 0;
  public movieData: MovieModel[] = [];

  dataSource = new MatTableDataSource<MovieModel>();
  @ViewChild('paginator') paginator: any = MatPaginator;
  pageSizes = [10, 50, 100];
  public genreList: string[] = [];
  public genre: string = '';
  public pageSize = 10;
  constructor(private moviesService: MovieService, private formBuilder: FormBuilder) { }
  ngOnInit(): void {
    this.moviesService.getGenre()
      .pipe(map((val) => val.map((d) => d.genre as string)
      ))
      .subscribe((val) => {
        this.genreList = val
      });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;

    this.paginator.page
      .pipe(
        startWith({}),
        switchMap(() => {
          
          this.pageSize = this.paginator.pageSize
          return this.moviesService.getMovies(this.searchStr, this.genre, this.paginator.pageSize, this.paginator.pageIndex + 1, 'Title');
        })
      )
      .subscribe((response:MoviePageModel) => {
        console.log(response);
        this.totalData = response.totalCount
        this.movieData = response.movies;
        this.dataSource = new MatTableDataSource(this.movieData);
      });

    this.searchForm.controls['nameFilter'].valueChanges.pipe(
      startWith(''),
      debounceTime(1000),
      distinctUntilChanged(),
      switchMap(name => {
        this.searchStr = name ?? '';

        return this.moviesService.getMovies(this.searchStr, this.genre, this.pageSize, this.paginator.pageIndex, 'Title')
      })
    )    .subscribe((response:MoviePageModel) => {
      console.log(response);
      this.totalData = response.totalCount
      this.movieData = response.movies;
      this.dataSource = new MatTableDataSource(this.movieData);
    });
  }

  filterByGenre(e: any) {
    this.searchForm.controls['genreFilter'].setValue(e.target.value, { onlySelf: true });
    this.genre = e.target.value !== 'All' ? e.target.value : '';
    this.moviesService.getMovies(this.searchStr, this.genre, this.pageSize, this.paginator.pageIndex, 'Title')
    .subscribe((response:MoviePageModel) => {
      console.log(response);
      this.totalData = response.totalCount
      this.movieData = response.movies;
      this.dataSource = new MatTableDataSource(this.movieData);
    });
  }
}


