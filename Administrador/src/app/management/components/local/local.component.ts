import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

import { AlertService } from "../../../core/services/alert.service";
import { ComunService } from '../../../core/services/comun.service';
import { StorageService } from 'src/app/core/services/storage.service';
import { globals } from '../../../core/globals/globals';
import { LocalService } from 'src/app/core/services/local.service';
import { Local } from 'src/app/classes/Local';
import { AutoUnsubscribe } from "ngx-auto-unsubscribe";

@AutoUnsubscribe()
@Component({
  selector: 'app-local',
  templateUrl: './local.component.html',
  styleUrls: ['./local.component.scss']
})
export class LocalComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['acciones', 'estatus', 'IdLocal', 'Nombre', 'Calle', 'Colonia'];
  dataSource : any;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  constructor(
    private _localService: LocalService,
    private mensajes: AlertService,
    private comunesService: ComunService,
    private storageService: StorageService,
    public globales: globals,
     ) { }

  public ngOnDestroy() {}

  ngOnInit() {
    setTimeout(() => {
      this.getLocales();
    });
  }

  /**
   * Función para filtrar contenido de la tabla
   */
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  /**
   * Inicializa opciones de la tabla, como paginador y ordenamiento
   */
  initOptionsMattable(){
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  /**
   * Cambia el estatus de un registro
   * @param pElement
   * @param pId
   */
  onChangeEstatus(pElement: any, pId: number) {
    //let request = new ActualizaEstatusGenericoRequest(pId, pElement.checked, this.storageService.getCurrentSession().user.idpersona, 'nodo');
    let request = new Local();
    request.IdLocal = pId;
    request.Estatus = pElement.checked ? 1 : 0;
    request.IdPersonaModifica = 1//this.storageService.getCurrentSession().user.idpersona;
    this._localService.actualizaEstatus(request).subscribe(
      respuesta => {
        if (respuesta.Exito) {
          this.getLocales()
        } else {
          pElement.checked = !pElement.checked;
          this.mensajes.showWarning(respuesta.Mensaje);
        }
      }, error => {
        this.mensajes.showError(error.message);
      })
  }

  /**
   * Consume servicio para consultar locales.
   */
  private getLocales() :void {
    this._localService.getLocales().subscribe(
      respuesta => {
        //console.log(respuesta)
        if (respuesta.Exito){
          this.dataSource = new MatTableDataSource<Local>(respuesta.Data);
          this.initOptionsMattable();
        }else
          this.mensajes.showWarning(respuesta.Mensaje);
      }, error => {
        this.mensajes.showError(error.message);
      });
  }
}

