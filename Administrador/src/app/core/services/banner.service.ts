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
import { CreaActualizaBannerRequest } from 'src/app/classes/request/CreaActualizaBannerRequest';

@Injectable({
  providedIn: 'root'
})
export class BannerService {

  private basePath: string;
  private objPeticion: any = {};

  constructor(private http: HttpClient, private globales: globals) {
    this.basePath = `${environment.BACKEND_BASE_URI}Banners/`;
  }


  /**
  * Comsume Web service para actualizar estatus de la persona
  * @param pEstatus - Objeto de tipo  ActualizaEstatusGenericoRequest con datos a actualizar.
  * @returns Observable de tipo Respuesta
  */
  actualizaEstatus(pEstatus: ActualizaEstatusGenericoRequest<string>): Observable<Respuesta> {
    return this.http.patch<Respuesta>(`${this.basePath}/CambiaEstatus`, pEstatus, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }


  /**
  * Comsume Web service para agregar persona/usuario
  * @param pBanner - Objeto de tipo  CreaActualizaBannerRequest con datos a agregar.
  * @returns Observable de tipo Respuesta
  */
  agregar(pBanner: CreaActualizaBannerRequest): Observable<Respuesta> {
    return this.http.post<Respuesta>(`${this.basePath}`, pBanner, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }

  /**
  * Comsume Web service para editar persona/usuario
  * @param pBanner - Objeto de tipo  CreaActualizaBannerRequest con datos a editar.
  * @returns Observable de tipo Respuesta
  */
  editar(pBanner: CreaActualizaBannerRequest): Observable<Respuesta> {
    return this.http.put<Respuesta>(`${this.basePath}`, pBanner, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }

  /**
 * Comsume Web service para consultar personas
 * @param pSoloActivos - Indica si consultar solo activos o no
 * * @param pTiposUsuarios - Indica el tipo de usuarios a consultar separados por coma
 * @returns Observable de tipo Respuesta
 */
  getBanners(pSoloActivos?: boolean): Observable<Respuesta> {
    let params = new HttpParams();
    if (pSoloActivos != undefined)
      params = params.append('soloActivos', ((pSoloActivos == true) ? 1 : 0).toString());

    this.globales.HTTP_OPTIONS.params = params;

    return this.http.get<Respuesta>(`${this.basePath}`, this.globales.HTTP_OPTIONS).pipe(
      //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
      map(res => res as Respuesta),
      //catchError(this.handleError<Sesion>('login', new Sesion()))
    );
  }


  /**
 * Comsume Web service para consultar un banner por id
 * @param pIdBanner - Indentificador del banner a consultar
 * @returns Observable de tipo Respuesta
 */
   getBannersById(pIdBanner: string): Observable<Respuesta> {
    return this.http.get<Respuesta>(`${this.basePath}/${pIdBanner}`, this.globales.HTTP_OPTIONS).pipe(
      map(res => res as Respuesta),
    );
  }



}
