import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
//Globales
import { globals } from '../globals/globals';
import { environment } from '../../../environments/environment';
import { Respuesta } from '../../classes/Respuesta';
import { GetRptGananciaListPaginatedResponse } from '../../classes/response/GetRptGananciaListPaginatedResponse';
import { GetRptGeneralListPaginatedResponse } from '../../classes/response/GetRptGeneralListPaginatedResponse';
import { PaginatedList } from 'src/app/classes/PaginatedList';
import { RptGanancia } from 'src/app/classes/RptGanancia';

@Injectable({
  providedIn: 'root'
})
export class ReportesService {

  private basePath: string;

  constructor(private http: HttpClient
    , private globales: globals) {
    this.basePath = `${environment.BACKEND_BASE_URI}Reportes/`;
  }


  /**
     * Devuelve una lista de pedidos, usando algunos parámetros.
     * @param pageIndex Número de Página.
     * @param pageSize - Tamaño de la Página.
     * @param sortColumn - Columna a ordenar.
     * @param sortDirection - Dirección de ordenamiento.
     * @param palabraClave - Palabra clave para filtrar.
     * @param fechaInicial - Fecha inicial de busqueda de pedidos.
     * @param fechaFinal - Fecha final de busqueda de pedidos.
     * @returns Observable de tipo GetRptGananciaListPaginatedResponse.
    */
  getListPaginatedGanancias(
    pageIndex = 0, pageSize = 10, sortColumn = '', sortDirection = 'asc',
    palabraClave = '', fechaInicial?: Date, fechaFinal?: Date
  ): Observable<GetRptGananciaListPaginatedResponse> {

    let url = this.basePath + 'RptGanancias';

    let params = new HttpParams();
    let httpOptions = this.globales.HTTP_OPTIONS;

    params = params.append('pageIndex', pageIndex.toString());
    params = params.append('pageSize', pageSize.toString());
    params = params.append('sortColumn', sortColumn);
    params = params.append('sortDirection', sortDirection);
    params = params.append('palabraClave', palabraClave);
    if (fechaInicial)
      params = params.append('fechaInicial', fechaInicial.toString());
    if (fechaFinal)
      params = params.append('fechaFinal', fechaFinal.toString());

    httpOptions.params = params;

    return this.http.get<GetRptGananciaListPaginatedResponse>(url, httpOptions).pipe(
      map(res => res as GetRptGananciaListPaginatedResponse),
      catchError(this.handleError<GetRptGananciaListPaginatedResponse>('getListPaginatedGanancia', new GetRptGananciaListPaginatedResponse()))
    );
  }

  /**
     * Devuelve una lista de pedidos, usando algunos parámetros.
     * @param pageIndex Número de Página.
     * @param pageSize - Tamaño de la Página.
     * @param sortColumn - Columna a ordenar.
     * @param sortDirection - Dirección de ordenamiento.
     * @param palabraClave - Palabra clave para filtrar.
   * @param fechaInicial - Fecha inicial de busqueda de pedidos.
     * @param fechaFinal - Fecha final de busqueda de pedidos.
     * @returns Observable de tipo GetRptGeneralListPaginatedResponse.
    */
   getListPaginatedGeneral(
    pageIndex = 0, pageSize = 10, sortColumn = '', sortDirection = 'asc',
    palabraClave = '', fechaInicial?: Date, fechaFinal?: Date
  ): Observable<GetRptGeneralListPaginatedResponse> {

    let url = this.basePath + 'RptGeneral';

    let params = new HttpParams();
    let httpOptions = this.globales.HTTP_OPTIONS;

    params = params.append('pageIndex', pageIndex.toString());
    params = params.append('pageSize', pageSize.toString());
    params = params.append('sortColumn', sortColumn);
    params = params.append('sortDirection', sortDirection);
    params = params.append('palabraClave', palabraClave);
    if (fechaInicial)
      params = params.append('fechaInicial', fechaInicial.toString());
    if (fechaFinal)
      params = params.append('fechaFinal', fechaFinal.toString());

    httpOptions.params = params;

    return this.http.get<GetRptGeneralListPaginatedResponse>(url, httpOptions).pipe(
      map(res => res as GetRptGeneralListPaginatedResponse),
      catchError(this.handleError<GetRptGeneralListPaginatedResponse>('getListPaginatedGeneral', new GetRptGeneralListPaginatedResponse()))
    );
  }

  // /**
  //  * Exporta datos a excel, usando algunos parámetros.
  //  * @param pageIndex Número de Página.
  //  * @param pageSize - Tamaño de la Página.
  //  * @param sortColumn - Columna a ordenar.
  //  * @param sortDirection - Dirección de ordenamiento.
  //  * @param palabraClave - Palabra clave para filtrar.
  //  * @param idTipoDependencia - Identificador de un tipo de dependencia.
  //  * @param idUnidadAdministrativa - Identidicador de una UA.
  //  * @returns Observable de tipo GetCentroCostoListPaginatedResponse.
  // */
  // getExcel(
  //   pageIndex = 0, pageSize = 10, sortColumn = '', sortDirection = 'asc',
  //   palabraClave = '', idTipoDependencia?: string, idUnidadAdministrativa?: string
  // ): Observable<Blob> {
  //   let params = new HttpParams();
  //   let httpOptions = this.globales.HTTP_OPTIONS;

  //   let url = this.basePath + 'exportar/excel';
  //   params = params.append('pageIndex', pageIndex.toString());
  //   params = params.append('pageSize', pageSize.toString());
  //   params = params.append('sortColumn', sortColumn);
  //   params = params.append('sortDirection', sortDirection);
  //   params = params.append('palabraClave', palabraClave);
  //   if (idTipoDependencia)
  //     params = params.append('idTipoDependencia', idTipoDependencia);
  //   if (idUnidadAdministrativa)
  //     params = params.append('idUnidadAdministrativa', idUnidadAdministrativa);

  //   httpOptions.params = params;

  //   return this.http.get<Blob>(url, { params, responseType: 'blob' as 'json' }).pipe(
  //     //tap(() => console.log("Petición HTTP para consultar nodos ejecutada")),
  //     map(res => res as Blob),
  //     catchError(this.handleError<Blob>('excel', new Blob()))
  //   );
  // }

  /**
 * Handle Http operation that failed.
 * Let the app continue.
 * @param operation - name of the operation that failed
 * @param result - optional value to return as the observable result
 */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      // console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      // console.log(`${operation} failed: ${error.message}`);

      if (result instanceof GetRptGananciaListPaginatedResponse) {
        result.Data = new PaginatedList<RptGanancia>();
        result.Data.Rows = new Array<RptGanancia>();
        result.Data.TotalRows = 0;
      }
      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

}
