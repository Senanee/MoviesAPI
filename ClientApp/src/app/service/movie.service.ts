import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { MOVIE_BASE_URL } from '../app.config';
import { GenreModel } from '../models/genre-model';
import { MovieModel, MoviePageModel } from '../models/movie-model';

export interface ODataResponse<T> {
  value: T[];
}

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  constructor(
    private httpClient: HttpClient,
    @Inject(MOVIE_BASE_URL) private baseUrl: string
  ) {}


  public getMovies(nameFilter?:string,genreFilter?:string, pageSize:number=10, pageNumber:number=1, orderBy:string='Title,ReleaseDate'):Observable<MoviePageModel>{
    let params = new HttpParams();
    if (nameFilter ) {
      params = params.set('name',nameFilter);
    }
    if (genreFilter) {
      params = params.set('genre',genreFilter);
    }
    pageNumber=pageNumber>0?pageNumber:1;
    params = params.set('page', pageNumber);
    params = params.set('limit', pageSize);
    params=params.set('sort',orderBy)


    return this.httpClient
      .get<MoviePageModel>(`${this.baseUrl}/GetMovies`, {params: params} );
  }
  public getGenre():Observable<GenreModel[]>{
    return this.httpClient
    .get<GenreModel[]>(`${this.baseUrl}/GetGenreList`);
  }
}
