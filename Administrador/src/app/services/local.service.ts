import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
//Globales
import { globals } from '../globals/globals';
import { environment } from '../../environments/environment';
import { Respuesta } from '../classes/Respuesta';
import { Local } from '../classes/Local';

@Injectable({
  providedIn: 'root'
})
export class LocalService {

  private basePath: string;
  private objPeticion: any = {};

  constructor(private http: HttpClient, private globales: globals) {
    this.basePath = `${environment.BACKEND_BASE_URI}Local/`;
  }

  /**
 * Comsume Web service para consultar locales
 * @param pSoloActivos - Indica si consultar solo activos o no
 * @returns Observable de tipo Respuesta
 */
  getLocales(pSoloActivos?: boolean): Observable<Respuesta> {
    let params = new HttpParams();
    if (pSoloActivos != undefined)
      params = params.append('soloActivos', ((pSoloActivos == true) ? 1 : 0).toString());
    this.globales.HTTP_OPTIONS.params = params;

    return this.http.get<Respuesta>(this.basePath, this.globales.HTTP_OPTIONS).pipe(
      //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
      map(res => res as Respuesta),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }

  /**
* Comsume Web service para consultar local por id
* @param pIdLocal - Id del tipo local a consultar
* @returns Observable de tipo Respuesta
*/
  getLocalByID(pIdLocal: number): Observable<Respuesta> {
    return this.http.get<Respuesta>(`${this.basePath}${pIdLocal}`, this.globales.HTTP_OPTIONS).pipe(
      //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
      map(res => res as Respuesta),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }

  /**
  * Comsume Web service para actualizar estatus del local
  * @param pEstatus - Objeto de tipo  Producto con datos a actualizar.
  * @returns Observable de tipo Respuesta
  */
  actualizaEstatus(pEstatus: Local): Observable<Respuesta> {
    this.objPeticion.Local = pEstatus;
    return this.http.put<Respuesta>(`${this.basePath}CambiaEstatus`, this.objPeticion, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }

  /**
* Comsume Web service para consultar tipos de locales
* @param pSoloActivos - Indica si consultar solo activos o no
* @returns Observable de tipo Respuesta
*/
  getTiposLocal(pSoloActivos?: boolean): Observable<Respuesta> {
    let params = new HttpParams();
    if (pSoloActivos != undefined)
      params = params.append('soloActivos', ((pSoloActivos == true) ? 1 : 0).toString());
    this.globales.HTTP_OPTIONS.params = params;

    return this.http.get<Respuesta>(`${this.basePath}TipoLocal`, this.globales.HTTP_OPTIONS).pipe(
      //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
      map(res => res as Respuesta),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }

  /**
* Comsume Web service para consultar tipos de local por id
* @param pIdTipoLocal - Id del tipo local a consultar
* @returns Observable de tipo Respuesta
*/
  getTipoLocalByID(pIdTipoLocal: number): Observable<Respuesta> {
    return this.http.get<Respuesta>(`${this.basePath}TipoLocal/${pIdTipoLocal}`, this.globales.HTTP_OPTIONS).pipe(
      //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
      map(res => res as Respuesta),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }


  /**
* Comsume Web service para consultar costos de locales
* @param pSoloActivos - Indica si consultar solo activos o no
* @returns Observable de tipo Respuesta
*/
  getCostos(pSoloActivos?: boolean): Observable<Respuesta> {
    let params = new HttpParams();
    if (pSoloActivos != undefined)
      params = params.append('soloActivos', ((pSoloActivos == true) ? 1 : 0).toString());
    this.globales.HTTP_OPTIONS.params = params;

    return this.http.get<Respuesta>(`${this.basePath}Costo`, this.globales.HTTP_OPTIONS).pipe(
      //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
      map(res => res as Respuesta),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }

  /**
  * Comsume Web service para consultar costos de local por id
  * @param pIdCosto - Id del tipo local a consultar
  * @returns Observable de tipo Respuesta
  */
  getCostosByID(pIdCosto: number): Observable<Respuesta> {
    return this.http.get<Respuesta>(`${this.basePath}Costo/${pIdCosto}`, this.globales.HTTP_OPTIONS).pipe(
      //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
      map(res => res as Respuesta),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }

  /**
  * Comsume Web service para agregar locales
  * @param pLocal - Objeto de tipo  Local con datos a agregar.
  * @returns Observable de tipo Respuesta
  */
  agregar(pLocal: Local): Observable<Respuesta> {
    this.objPeticion.Local = pLocal;
    console.log(this.objPeticion)
    return this.http.post<Respuesta>(`${this.basePath}`, this.objPeticion, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }

  /**
  * Comsume Web service para actualizar locales
  * @param pLocal - Objeto de tipo  Producto con datos a actualizar.
  * @returns Observable de tipo Respuesta
  */
  editar(pLocal: Local): Observable<Respuesta> {
    this.objPeticion.Local = pLocal;
    return this.http.put<Respuesta>(`${this.basePath}`, this.objPeticion, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }
}
