import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
//Globales
import { globals } from '../globals/globals';
import { environment } from '../../environments/environment';
import { ConsultaNodosResponse } from '../classes/response/ConsultaNodosResponse';

@Injectable({
  providedIn: 'root'
})
export class NodosService {

  private basePath: string;

  constructor(private http: HttpClient, private globales: globals) {
    this.basePath = `${environment.BACKEND_BASE_URI}nodos/`;
  }

  /**
 * Comsume Web service para consultar nodos
 * @pIdNodo Indica el nodo del cual se quieren consultar las estrategias.
 * @param pSoloActivos - Indica si consultar solo activos o no
 * @returns Observable de tipo ConsultaNodosResponse
 */
  getNodos(pIdNodo?: number, pSoloActivos?: boolean): Observable<ConsultaNodosResponse> {
    let params = new HttpParams();
    if (pIdNodo != undefined)
      params = params.append('pIdNodo', pIdNodo.toString());
    if (pSoloActivos != undefined)
      params = params.append('pSoloActivos', ((pSoloActivos == true) ? 1 : 0).toString());
    this.globales.HTTP_OPTIONS.params = params;

    return this.http.get<ConsultaNodosResponse>(this.basePath, this.globales.HTTP_OPTIONS).pipe(
      //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
      map(res => res as ConsultaNodosResponse),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }
}
