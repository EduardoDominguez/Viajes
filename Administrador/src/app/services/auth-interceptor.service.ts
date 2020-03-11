import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';

import { StorageService } from './storage.service';
import { LoadingService } from './loading.service';
import { finalize, tap } from 'rxjs/operators';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {
  constructor(private storage: StorageService,
    private loading: LoadingService) {
    //console.log(storage);
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    req = req.clone({
      setHeaders: {
        'Content-Type': 'application/json; charset=utf-8',
        'Accept': 'application/json',
        'Authorization': `Bearer ${this.storage.getCurrentToken()}`,
      },
    });

    return next.handle(req).pipe(
      tap(() => this.loading.toggle(true)),
      finalize(() => this.loading.toggle(false))
    );
  }
}