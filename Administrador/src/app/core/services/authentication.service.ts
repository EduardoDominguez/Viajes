import {Injectable} from "@angular/core";
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import {Sesion} from "../../classes/Sesion";
import {LoginRequest} from "../../classes/request/LoginRequest";

//Globales
import {globals} from '../../core/globals/globals';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  //private basePath = 'http://localhost:3000/autenticar/';
  //private basePath = 'autenticar/';
  private basePath : string;


  constructor( private http: HttpClient, private globales: globals)
  {
      this.basePath = environment.BACKEND_BASE_URI+'Usuario/';
  }

  /** POST: add a new hero to the server */
  login (loginObj: LoginRequest): Observable<Sesion> {
    return this.http.post<Sesion>(this.basePath+ 'Login', loginObj, this.globales.HTTP_OPTIONS).pipe(
      tap(() =>console.log("Petición HTTP ejecutada")),
      map(res  =>  res as Sesion),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }

 logout(): Observable<Boolean> {
    //return this.http.post(this.basePath + 'logout', {}).map(this.extractData);
    return this.http.post<Boolean>(this.basePath+ 'login', {}, this.globales.HTTP_OPTIONS).pipe(
      tap(_=>console.log('sesión cerrada')),
      catchError(this.handleError<Boolean>('logout'))
    );
 }

 recuperarPassword (pParametros: any): Observable<any> {
  return this.http.post<any>(this.basePath+ 'recuperar-password', pParametros, this.globales.HTTP_OPTIONS).pipe(
    tap(() =>console.log("Petición HTTP ejecutada")),
    map(res  =>  res as any),
    //catchError(this.handleError<Sesion>('login', new Sesion()))
  );
}

 private extractData(res: Response) {
    let body = res.json();
    return body;
 }

  /**
 * Handle Http operation that failed.
 * Let the app continue.
 * @param operation - name of the operation that failed
 * @param result - optional value to return as the observable result
 */
  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      console.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
