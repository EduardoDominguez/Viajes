import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Location } from '@angular/common';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

import { AlertService } from "../../../core/services/alert.service";
import { StorageService } from 'src/app/core/services/storage.service';
import { globals } from '../../../core/globals/globals';
import { ActivatedRoute, Router } from '@angular/router';
import { AutoUnsubscribe } from "ngx-auto-unsubscribe";
import { TipoUsuarioEnum } from 'src/app/classes/enums/TipoUsuarioEnum';
import { ActualizaEstatusGenericoRequest } from 'src/app/classes/request/ActualizaEstatusGenericoRequest';
import { TipoLocal } from 'src/app/classes/TipoLocal';
import { LocalService } from 'src/app/core/services/local.service';



@AutoUnsubscribe()
@Component({
  selector: 'app-tipo-local',
  templateUrl: './tipo-local.component.html',
  styleUrls: ['./tipo-local.component.scss']
})
export class TipoLocalComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = [/*'acciones', 'estatus', */'Fotografia', 'Nombre' ];
  dataSource: any;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  idLocal: number = 0;
  nombreLocal: string = "";
  constructor(
    private _localService: LocalService,
    private mensajes: AlertService,
    private storageService: StorageService,
    public globales: globals,
    private router: Router,
    private route: ActivatedRoute,
    private _location: Location
  ) {
    this.procesaRutas();
  }

  public ngOnDestroy() { }

  ngOnInit() {
    setTimeout(() => {
      this.getCategorias();
    });

  }

  /**
   * Detecta los parametros de la ruta
   */
  procesaRutas() {
    this.idLocal = +this.route.snapshot.paramMap.get('idLocal');
    this.route.queryParamMap.subscribe(queryParams => {
      this.nombreLocal = (queryParams.get("name") == null) ? "" : queryParams.get("name");
    });
  }

  goBack(): void {
    this._location.back();
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
  initOptionsMattable() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  /**
   * Cambia el estatus de un registro
   * @param pElement
   * @param pId
   */
  onChangeEstatus(pElement: any, pId: number) {
    // let request = new ActualizaEstatusGenericoRequest(pId, pElement.checked ? 1 : 0, this.storageService.getCurrentSession().Persona.IdPersona);
    // this._localService.actualizaEstatus(request).subscribe(
    //   respuesta => {
    //     if (respuesta.Exito) {
    //       this.getCategorias();
    //       this.mensajes.showSuccess(respuesta.Mensaje);
    //     } else {
    //       pElement.checked = !pElement.checked;
    //       this.mensajes.showWarning(respuesta.Mensaje);
    //     }
    //   }, error => {
    //     this.mensajes.showError(error.message);
    //   })
  }

  /**
   * Consume servicio para consultar usuarios.
   */
  public getCategorias(): void {
    this._localService.getTiposLocal().subscribe(
      respuesta => {
        // console.log(respuesta)
        if (respuesta.Exito) {
          this.dataSource = new MatTableDataSource<TipoLocal>(respuesta.Data);
          this.initOptionsMattable();
        } else
          this.mensajes.showWarning(respuesta.Mensaje);
      }, error => {
        this.mensajes.showError(error.message);
      });
  }

  public get TipoUsuario(){
    return TipoUsuarioEnum;
  }
}

