import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
//Globales
import { globals } from '../globals/globals';
import { environment } from '../../../environments/environment';
import { Respuesta } from '../../classes/Respuesta';
import { CancelaPedidoRequest } from 'src/app/classes/request/CancelaPedidoRequest';

@Injectable({
  providedIn: 'root'
})
export class PedidoService {

  private basePath: string;
  private objPeticion: any = {};

  constructor(private http: HttpClient, private globales: globals) {
    this.basePath = `${environment.BACKEND_BASE_URI}Pedido/`;
  }

  /**
 * Comsume Web service para consultar pedidos
 * @returns Observable de tipo Respuesta
 */
  getPedidos(): Observable<Respuesta> {
    let params = new HttpParams();
    // if (pSoloActivos != undefined)
    //   params = params.append('soloActivos', ((pSoloActivos == true) ? 1 : 0).toString());
    // this.globales.HTTP_OPTIONS.params = params;

    return this.http.get<Respuesta>(this.basePath, this.globales.HTTP_OPTIONS).pipe(
      //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
      map(res => res as Respuesta),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }

  /**
* Comsume Web service para consultar pedido por id
* @param pIdPedido - Id del tipo local a consultar
* @returns Observable de tipo Respuesta
*/
getPedidoByID(pIdPedido: string): Observable<Respuesta> {
    return this.http.get<Respuesta>(`${this.basePath}${pIdPedido}`, this.globales.HTTP_OPTIONS).pipe(
      //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
      map(res => res as Respuesta),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }

  /**
  * Comsume Web service para cancelar un pedido
  * @param pPeticion - Objeto de tipo  CancelaPedidoRequest con datos a actualizar.
  * @returns Observable de tipo Respuesta
  */
  cancelar(pPeticion: CancelaPedidoRequest): Observable<Respuesta> {
    this.objPeticion.Pedido = pPeticion;
    return this.http.put<Respuesta>(`${this.basePath}Cancelar`, this.objPeticion, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }

//   /**
// * Comsume Web service para consultar tipos de locales
// * @param pSoloActivos - Indica si consultar solo activos o no
// * @returns Observable de tipo Respuesta
// */
//   getTiposLocal(pSoloActivos?: boolean): Observable<Respuesta> {
//     let params = new HttpParams();
//     if (pSoloActivos != undefined)
//       params = params.append('soloActivos', ((pSoloActivos == true) ? 1 : 0).toString());
//     this.globales.HTTP_OPTIONS.params = params;

//     return this.http.get<Respuesta>(`${this.basePath}TipoLocal`, this.globales.HTTP_OPTIONS).pipe(
//       //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
//       map(res => res as Respuesta),
//       //catchError(this.handleError<Sesion>('login', new Sesion()))
//     );
//   }

//   /**
// * Comsume Web service para consultar tipos de local por id
// * @param pIdTipoLocal - Id del tipo local a consultar
// * @returns Observable de tipo Respuesta
// */
//   getTipoLocalByID(pIdTipoLocal: number): Observable<Respuesta> {
//     return this.http.get<Respuesta>(`${this.basePath}TipoLocal/${pIdTipoLocal}`, this.globales.HTTP_OPTIONS).pipe(
//       //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
//       map(res => res as Respuesta),
//       //catchError(this.handleError<Sesion>('login', new Sesion()))
//     );
//   }


//   /**
// * Comsume Web service para consultar costos de locales
// * @param pSoloActivos - Indica si consultar solo activos o no
// * @returns Observable de tipo Respuesta
// */
//   getCostos(pSoloActivos?: boolean): Observable<Respuesta> {
//     let params = new HttpParams();
//     if (pSoloActivos != undefined)
//       params = params.append('soloActivos', ((pSoloActivos == true) ? 1 : 0).toString());
//     this.globales.HTTP_OPTIONS.params = params;

//     return this.http.get<Respuesta>(`${this.basePath}Costo`, this.globales.HTTP_OPTIONS).pipe(
//       //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
//       map(res => res as Respuesta),
//       //catchError(this.handleError<Sesion>('login', new Sesion()))
//     );
//   }

//   /**
//   * Comsume Web service para consultar costos de local por id
//   * @param pIdCosto - Id del tipo local a consultar
//   * @returns Observable de tipo Respuesta
//   */
//   getCostosByID(pIdCosto: number): Observable<Respuesta> {
//     return this.http.get<Respuesta>(`${this.basePath}Costo/${pIdCosto}`, this.globales.HTTP_OPTIONS).pipe(
//       //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
//       map(res => res as Respuesta),
//       //catchError(this.handleError<Sesion>('login', new Sesion()))
//     );
//   }

//   /**
//   * Comsume Web service para agregar locales
//   * @param pLocal - Objeto de tipo  Local con datos a agregar.
//   * @returns Observable de tipo Respuesta
//   */
//   agregar(pLocal: Local): Observable<Respuesta> {
//     this.objPeticion.Local = pLocal;
//     console.log(this.objPeticion)
//     return this.http.post<Respuesta>(`${this.basePath}`, this.objPeticion, this.globales.HTTP_OPTIONS).pipe(
//       map(res => res as Respuesta),
//     );
//   }

//   /**
//   * Comsume Web service para actualizar locales
//   * @param pLocal - Objeto de tipo  Producto con datos a actualizar.
//   * @returns Observable de tipo Respuesta
//   */
//   editar(pLocal: Local): Observable<Respuesta> {
//     this.objPeticion.Local = pLocal;
//     return this.http.put<Respuesta>(`${this.basePath}`, this.objPeticion, this.globales.HTTP_OPTIONS).pipe(
//       map(res => res as Respuesta),
//     );
//   }
}
