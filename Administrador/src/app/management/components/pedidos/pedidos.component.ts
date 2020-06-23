import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

import { AlertService } from "../../../core/services/alert.service";
// import { StorageService } from 'src/app/core/services/storage.service';
import { globals } from '../../../core/globals/globals';
import { PedidoService } from 'src/app/core/services/pedido.service';
import { Pedido } from 'src/app/classes/Pedido';
import { AutoUnsubscribe } from "ngx-auto-unsubscribe";

@AutoUnsubscribe()
@Component({
  selector: 'app-pedidos',
  templateUrl: './pedidos.component.html',
  styleUrls: ['./pedidos.component.scss']
})
export class PedidosComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['acciones', 'estatus', 'folio', 'fechaPedido', 'personaPide', 'direccionEntrega', 'personaEntrega', 'costoEnvio', 'metodoPago', 'referencia', 'observaciones'];
  dataSource: any;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  objConsultaPedidoSuscriber: any;

  constructor(
    private _pedidoService: PedidoService,
    private mensajes: AlertService,
    // private storageService: StorageService,
    public globales: globals,
  ) { }

  ngOnInit() {
    setTimeout(() => {
      this.getPedidos();
    });
  }

  ngOnDestroy() {
    if (this.objConsultaPedidoSuscriber != null)
      this.objConsultaPedidoSuscriber.unsubscribe();
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
   * Consume servicio para consultar locales.
   */
  private getPedidos(): void {
    this.objConsultaPedidoSuscriber = this._pedidoService.getPedidos().subscribe(
      respuesta => {
        console.log(respuesta)
        if (respuesta.Exito) {
          this.dataSource = new MatTableDataSource<Pedido>(respuesta.Data);
          this.initOptionsMattable();
        } else
          this.mensajes.showWarning(respuesta.Mensaje);
      }, error => {
        this.mensajes.showError(error.message);
      });
  }
}

