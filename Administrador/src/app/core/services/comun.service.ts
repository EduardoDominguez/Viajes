import { Injectable } from '@angular/core';


import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
//Globales
import { globals } from '../globals/globals';
import { environment } from '../../../environments/environment';
import { IngresaMenuResponse } from '../../classes/response/IngresaMenuResponse';
import { ActualizaEstatusGenericoRequest } from '../../classes/request/ActualizaEstatusGenericoRequest';
@Injectable({
  providedIn: 'root'
})
export class ComunService {

  private basePath: string;

  constructor(private http: HttpClient, private globales: globals) {
    this.basePath = `${environment.BACKEND_BASE_URI}comunes/`;
  }
  /**
* Comsume Web service para actualizar estatus del menu
* @param pEstatus - Objeto de tipo  ActualizaEstatusGenericoRequest con datos a actualizar.
* @returns Observable de tipo IngresaMenuResponse
*/
  actualizaEstatus(pEstatus: ActualizaEstatusGenericoRequest): Observable<IngresaMenuResponse> {
    return this.http.put<IngresaMenuResponse>(`${this.basePath}estatus`, pEstatus, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as IngresaMenuResponse),
    );
  }
}
