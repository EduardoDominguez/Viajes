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

@Injectable({
  providedIn: 'root'
})
export class ProductoService {

  private basePath: string;
  private objPeticion: any = {};

  constructor(private http: HttpClient, private globales: globals) {
    this.basePath = `${environment.BACKEND_BASE_URI}Producto/`;
  }

  /**
 * Comsume Web service para consultar productos
 * @param pSoloActivos - Indica si consultar solo activos o no
 * @returns Observable de tipo Respuesta
 */
  getProductos(pSoloActivos?: boolean): Observable<Respuesta> {
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
 * Comsume Web service para consultar productos
 * @param pIdProducto - Producto a consultar
 * @returns Observable de tipo Respuesta
 */
  getProductoByID(pIdProducto: number): Observable<Respuesta> {
    return this.http.get<Respuesta>(`${this.basePath}${pIdProducto}`, this.globales.HTTP_OPTIONS).pipe(
      //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
      map(res => res as Respuesta),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }


  /**
 * Comsume Web service para consultar productos por id local
 * @param pIdLocal - Local a consultar
 * @returns Observable de tipo Respuesta
 */
  getProductoByIdLocal(pIdLocal: number): Observable<Respuesta> {
    return this.http.get<Respuesta>(`${this.basePath}local/${pIdLocal}`, this.globales.HTTP_OPTIONS).pipe(
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
  actualizaEstatus(pEstatus: Producto): Observable<Respuesta> {
    this.objPeticion.Producto = pEstatus;
    return this.http.put<Respuesta>(`${this.basePath}CambiaEstatus`, this.objPeticion, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }


  /**
  * Comsume Web service para agregar producto
  * @param pProducto - Objeto de tipo  Producto con datos a actualizar.
  * @returns Observable de tipo Respuesta
  */
  agregar(pProducto: Producto): Observable<Respuesta> {
    this.objPeticion.Producto = pProducto;
    return this.http.post<Respuesta>(`${this.basePath}`, this.objPeticion, this.globales.HTTP_OPTIONS).pipe(
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
  editarExtras(pExtra: ExtraProductoRequest): Observable<Respuesta> {
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
    let params = new HttpParams();
    params = params.append('soloActivos','3');
    this.globales.HTTP_OPTIONS.params = params;

    return this.http.get<Respuesta>(`${this.basePath}${pIdProducto}/extras`, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }
}
