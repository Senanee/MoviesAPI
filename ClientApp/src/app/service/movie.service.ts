import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { MOVIE_BASE_URL } from '../app.config';
import { GenreModel } from '../models/genre-model';
import { MovieModel } from '../models/movie-model';

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


  public getMovies(nameFilter?:string,genreFilter?:string, pageSize:number=10, pageNumber:number=1, orderBy:string='Title'):Observable<MovieModel[]>{
    let params = new HttpParams();
    if (nameFilter && genreFilter) {
      params = params.set(
        '$filter',
        `contains(Title, '${nameFilter}') and contains(Genre, '${genreFilter}')`
      );
    }
    else if(nameFilter && !genreFilter){
      params = params.set(
        '$filter',
        `contains(Title, '${nameFilter}')`
      );
    }
    else if(!nameFilter && genreFilter){
      params = params.set(
        '$filter',
        `contains(Genre, '${genreFilter}')`
      );
    }
    let skipCount= (pageNumber-1)* pageSize;
    //params = params.set('$count', 'true');
    params = params.set('$top', pageSize);
    if(skipCount>0){
      params = params.set('$skip', skipCount);
    }
    
    params = params.set('$orderby', orderBy);

    return this.httpClient
      .get<MovieModel[]>(`${this.baseUrl}/GetMovies`, { params });
  }
  public getGenre():Observable<GenreModel[]>{
    return this.httpClient
    .get<GenreModel[]>(`${this.baseUrl}/GetGenreList`);
  }
}
