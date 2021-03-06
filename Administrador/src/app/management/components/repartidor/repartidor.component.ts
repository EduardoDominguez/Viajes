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
import { Repartidor } from 'src/app/classes/Repartidor';
import { PersonaService } from 'src/app/core/services/persona.service';
import { ActualizaEstatusGenericoRequest } from 'src/app/classes/request/ActualizaEstatusGenericoRequest';

@AutoUnsubscribe()
@Component({
  selector: 'app-repartidor',
  templateUrl: './repartidor.component.html',
  styleUrls: ['./repartidor.component.scss']
})
export class RepartidorComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['acciones', 'estatus', 'Fotografia', 'Nombre', 'Tipo'];
  dataSource: any;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  idLocal: number = 0;
  nombreLocal: string = "";
  constructor(
    private _repartidorService: PersonaService,
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
      this.getRepartidores();
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
    let request = new ActualizaEstatusGenericoRequest<number>(pId, pElement.checked ? 1 : 0, this.storageService.getCurrentSession().Persona.IdPersona);
    this._repartidorService.actualizaEstatus(request).subscribe(
      respuesta => {
        if (respuesta.Exito) {
          this.getRepartidores();
          this.mensajes.showSuccess(respuesta.Mensaje);

        } else {
          pElement.checked = !pElement.checked;
          this.mensajes.showWarning(respuesta.Mensaje);
        }
      }, error => {
        this.mensajes.showError(error.message);
      })
  }

  /**
   * Consume servicio para consultar repartidores.
   */
  public getRepartidores(): void {
    this._repartidorService.getRepartidores().subscribe(
      respuesta => {
        // console.log(respuesta)
        if (respuesta.Exito) {
          this.dataSource = new MatTableDataSource<Repartidor>(respuesta.Data);
          this.initOptionsMattable();
        } else
          this.mensajes.showWarning(respuesta.Mensaje);
      }, error => {
        this.mensajes.showError(error.message);
      });
  }
}

