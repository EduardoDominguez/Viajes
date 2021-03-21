import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
//Globales
import { globals } from '../globals/globals';
import { environment } from '../../../environments/environment';
import { Respuesta } from '../../classes/Respuesta';
import { Producto } from '../../classes/Producto';
import { ExtraProductoRequest } from 'src/app/classes/request/ExtraProductoRequest';
import { Repartidor } from 'src/app/classes/Repartidor';
import { EnviarNotificacionFirebaseRequest } from 'src/app/classes/request/EnviarNotifiacionFirebaseRequest';

@Injectable({
  providedIn: 'root'
})
export class PersonaService {

  private basePath: string;
  private objPeticion: any = {};

  constructor(private http: HttpClient, private globales: globals) {
    this.basePath = `${environment.BACKEND_BASE_URI}Persona/`;
  }

  /**
 * Comsume Web service para consultar productos
 * @param pSoloActivos - Indica si consultar solo activos o no
 * @returns Observable de tipo Respuesta
 */
  // getProductos(pSoloActivos?: boolean): Observable<Respuesta> {
  //   let params = new HttpParams();
  //   if (pSoloActivos != undefined)
  //     params = params.append('soloActivos', ((pSoloActivos == true) ? 1 : 0).toString());
  //   this.globales.HTTP_OPTIONS.params = params;

  //   return this.http.get<Respuesta>(this.basePath, this.globales.HTTP_OPTIONS).pipe(
  //     //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
  //     map(res => res as Respuesta),
  //     //catchError(this.handleError<Sesion>('login', new Sesion()))
  //   );
  // }

  /**
 * Comsume Web service para consultar personas
 * @param pIdPersona - Producto a consultar
 * @returns Observable de tipo Respuesta
 */
  getPersonaByID(pIdPersona: number): Observable<Respuesta> {
    return this.http.get<Respuesta>(`${this.basePath}${pIdPersona}`, this.globales.HTTP_OPTIONS).pipe(
      //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
      map(res => res as Respuesta),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }

  /**
  * Comsume Web service para actualizar estatus del producto
  * @param pEstatus - Objeto de tipo  Producto con datos a actualizar.
  * @returns Observable de tipo Respuesta
  */
  actualizaEstatusRepartidor(pEstatus: Repartidor): Observable<Respuesta> {
    this.objPeticion.Producto = pEstatus;
    return this.http.put<Respuesta>(`${this.basePath}Repartidor/CambiaEstatus`, this.objPeticion, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }


  // /**
  // * Comsume Web service para agregar producto
  // * @param pProducto - Objeto de tipo  Producto con datos a actualizar.
  // * @returns Observable de tipo Respuesta
  // */
  // agregar(pProducto: Producto): Observable<Respuesta> {
  //   this.objPeticion.Producto = pProducto;
  //   return this.http.post<Respuesta>(`${this.basePath}`, this.objPeticion, this.globales.HTTP_OPTIONS).pipe(
  //     map(res => res as Respuesta),
  //   );
  // }


  /**
 * Comsume Web service para consultar repartidores
 * @param pSoloActivos - Indica si consultar solo activos o no
 * @returns Observable de tipo Respuesta
 */
  getRepartidores(pSoloActivos?: boolean): Observable<Respuesta> {
    let params = new HttpParams();
    if (pSoloActivos != undefined)
      params = params.append('soloActivos', ((pSoloActivos == true) ? 1 : 0).toString());
    this.globales.HTTP_OPTIONS.params = params;

    return this.http.get<Respuesta>(`${this.basePath}Conductor/Consulta`, this.globales.HTTP_OPTIONS).pipe(
      //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
      map(res => res as Respuesta),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }

  /**
  * Comsume Web service para agregar repartidor
  * @param pRepartidor - Objeto de tipo  Producto con datos a actualizar.
  * @returns Observable de tipo Respuesta
  */
  agregarRepartidor(pRepartidor: Repartidor): Observable<Respuesta> {
    // this.objPeticion.Producto = pProducto;
    return this.http.post<Respuesta>(`${this.basePath}Conductor/Agrega`, pRepartidor, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }

  /**
   * Comsume Web service para actualizar producto
   * @param pProducto - Objeto de tipo  Producto con datos a actualizar.
   * @returns Observable de tipo Respuesta
   */
  editarRepartidor(pRepartidor: Repartidor): Observable<Respuesta> {
    // this.objPeticion.Producto = pProducto;
    return this.http.put<Respuesta>(`${this.basePath}Conductor/Edita`, pRepartidor, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }

  /**
 * Comsume Web service para actualizar producto
 * @param pProducto - Objeto de tipo  Producto con datos a actualizar.
 * @returns Observable de tipo Respuesta
 */
  editar(pProducto: Producto): Observable<Respuesta> {
    this.objPeticion.Producto = pProducto;
    return this.http.put<Respuesta>(`${this.basePath}`, this.objPeticion, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }


  /**
  * Comsume Web service para agregar extras a producto
  * @param pExtra - Objeto de tipo  ExtraProductoRequest con datos a insertar.
  * @returns Observable de tipo Respuesta
  */
  agregarExtras(pExtra: ExtraProductoRequest): Observable<Respuesta> {
    return this.http.post<Respuesta>(`${this.basePath}Extras`, pExtra, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }

  /**
  * Comsume Web service para editar extras a producto
  * @param pExtra - Objeto de tipo  ExtraProductoRequest con datos a insertar.
  * @returns Observable de tipo Respuesta
  */
  aditarExtras(pExtra: ExtraProductoRequest): Observable<Respuesta> {
    return this.http.put<Respuesta>(`${this.basePath}Extras`, pExtra, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }

  /**
 * Comsume Web service para consultar extras por producto
 * @param pIdProducto - Producto a consultar
 * @returns Observable de tipo Respuesta
 */
  getExtrasByIdProducto(pIdProducto: number): Observable<Respuesta> {
    return this.http.get<Respuesta>(`${this.basePath}${pIdProducto}/extras`, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }


  /**
  * Comsume Web service para enviar una notificación de FIrebase
  * @param pNotificacion - Objeto de tipo  EnviarNotificacionFirebaseRequest con datos del mensaje.
  * @returns Observable de tipo Respuesta
  */
   enviarNotificacionFirebase(pNotifiacion: EnviarNotificacionFirebaseRequest): Observable<Respuesta> {
    return this.http.post<Respuesta>(`${environment.BACKEND_BASE_URI}Notificacion/`, pNotifiacion, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }

}
