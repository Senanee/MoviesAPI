import { Routes } from '@angular/router';
import { MovieListComponent } from './movie-list/movie-list.component';

export const routes: Routes = [
    {path:'', redirectTo:'movies' , pathMatch:'full'},
    {path:'movies',component:MovieListComponent}
];
