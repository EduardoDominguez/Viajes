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
import { ActualizaPasswordRequest } from 'src/app/classes/request/ActualizaPasswordRequest';
import { CreaActualizaUsuarioRequest } from 'src/app/classes/request/CreaActualizaUsuarioRequest';
import { ActualizaEstatusGenericoRequest } from 'src/app/classes/request/ActualizaEstatusGenericoRequest';

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
  * Comsume Web service para actualizar estatus de la persona
  * @param pEstatus - Objeto de tipo  ActualizaEstatusGenericoRequest con datos a actualizar.
  * @returns Observable de tipo Respuesta
  */
  actualizaEstatus(pEstatus: ActualizaEstatusGenericoRequest): Observable<Respuesta> {
    return this.http.patch<Respuesta>(`${this.basePath}/ActualizaEstatus`, pEstatus, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }


  /**
  * Comsume Web service para agregar persona/usuario
  * @param pUsuario - Objeto de tipo  CreaActualizaUsuarioRequest con datos a agregar.
  * @returns Observable de tipo Respuesta
  */
  agregar(pUsuario: CreaActualizaUsuarioRequest): Observable<Respuesta> {
    return this.http.post<Respuesta>(`${this.basePath}Registro`, pUsuario, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }

  /**
  * Comsume Web service para editar persona/usuario
  * @param pUsuario - Objeto de tipo  CreaActualizaUsuarioRequest con datos a editar.
  * @returns Observable de tipo Respuesta
  */
  editar(pUsuario: CreaActualizaUsuarioRequest): Observable<Respuesta> {
    return this.http.put<Respuesta>(`${this.basePath}Actualiza`, pUsuario, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }



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
 * Comsume Web service para consultar personas
 * @param pSoloActivos - Indica si consultar solo activos o no
 * * @param pTiposUsuarios - Indica el tipo de usuarios a consultar separados por coma
 * @returns Observable de tipo Respuesta
 */
  getPersonas(pSoloActivos?: boolean, pTiposUsuarios?: string): Observable<Respuesta> {
    let params = new HttpParams();
    if (pSoloActivos != undefined)
      params = params.append('soloActivos', ((pSoloActivos == true) ? 1 : 0).toString());
    if (pTiposUsuarios != undefined)
      params = params.append('tipoUsuario', pTiposUsuarios);

    this.globales.HTTP_OPTIONS.params = params;

    return this.http.get<Respuesta>(`${this.basePath}Consulta`, this.globales.HTTP_OPTIONS).pipe(
      //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
      map(res => res as Respuesta),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }


  /**
 * Comsume Web service para consultar una persona por id
 * @param pIdPErsona - Indentificador de la persona a consultar
 * @returns Observable de tipo Respuesta
 */
  getPersonaById(pIdPErsona: number): Observable<Respuesta> {
    return this.http.get<Respuesta>(`${this.basePath}Consulta/${pIdPErsona}`, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }

  
  /**
 * Comsume Web service para consultar una persona por id
 * @param pIdPErsona - Indentificador de la persona a consultar
 * @returns Observable de tipo Respuesta
 */
   getRepartidorById(pIdPErsona: number): Observable<Respuesta> {
    return this.http.get<Respuesta>(`${this.basePath}Conductor/Consulta/${pIdPErsona}`, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
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
    return this.http.put<Respuesta>(`${this.basePath}Conductor/Actualiza`, pRepartidor, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }

  //   /**
  //  * Comsume Web service para actualizar producto
  //  * @param pProducto - Objeto de tipo  Producto con datos a actualizar.
  //  * @returns Observable de tipo Respuesta
  //  */
  //   editar(pProducto: Producto): Observable<Respuesta> {
  //     this.objPeticion.Producto = pProducto;
  //     return this.http.put<Respuesta>(`${this.basePath}`, this.objPeticion, this.globales.HTTP_OPTIONS).pipe(
  //       map(res => res as Respuesta),
  //     );
  //   }


  // /**
  // * Comsume Web service para agregar extras a producto
  // * @param pExtra - Objeto de tipo  ExtraProductoRequest con datos a insertar.
  // * @returns Observable de tipo Respuesta
  // */
  // agregarExtras(pExtra: ExtraProductoRequest): Observable<Respuesta> {
  //   return this.http.post<Respuesta>(`${this.basePath}Extras`, pExtra, this.globales.HTTP_OPTIONS).pipe(
  //     map(res => res as Respuesta),
  //   );
  // }

  // /**
  // * Comsume Web service para editar extras a producto
  // * @param pExtra - Objeto de tipo  ExtraProductoRequest con datos a insertar.
  // * @returns Observable de tipo Respuesta
  // */
  // aditarExtras(pExtra: ExtraProductoRequest): Observable<Respuesta> {
  //   return this.http.put<Respuesta>(`${this.basePath}Extras`, pExtra, this.globales.HTTP_OPTIONS).pipe(
  //     map(res => res as Respuesta),
  //   );
  // }

  //   /**
  //  * Comsume Web service para consultar extras por producto
  //  * @param pIdProducto - Producto a consultar
  //  * @returns Observable de tipo Respuesta
  //  */
  //   getExtrasByIdProducto(pIdProducto: number): Observable<Respuesta> {
  //     return this.http.get<Respuesta>(`${this.basePath}${pIdProducto}/extras`, this.globales.HTTP_OPTIONS).pipe(
  //       map(res => res as Respuesta),
  //     );
  //   }


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
