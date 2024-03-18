export interface MovieModel {
    id:number;
    title:string;
    releaseDate?: Date;
    overview?: string;
    popularity?:number;
    voteCount?: number;
    voteAverage?: number;
    originalLanguage?: string;
    genre?: string;
    posterUrl?:string;
}
