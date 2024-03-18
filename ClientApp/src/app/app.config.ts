import { provideHttpClient } from '@angular/common/http';
import { ApplicationConfig, InjectionToken } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

export const MOVIE_BASE_URL = new InjectionToken<string>('MOVIE_BASE_URL');

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withComponentInputBinding()),
    {
      provide: MOVIE_BASE_URL,
      useValue: 'http://localhost:3780/api/Movies',
    },
    provideHttpClient(), provideAnimationsAsync('noop'), provideAnimationsAsync('noop'),
  ],
};
