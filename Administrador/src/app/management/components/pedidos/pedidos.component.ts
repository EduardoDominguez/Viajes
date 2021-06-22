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
import { GenericConfirmDialogComponent } from 'src/app/management/modals/generic-confirm-dialog/generic-confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { CancelaPedidoRequest } from 'src/app/classes/request/CancelaPedidoRequest';

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
    public _dialogService: MatDialog
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
  public getPedidos(): void {
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

  /**
   * Abre el panel para confirmar movimiento de cancelar pedido
   */
   public cancelarPedido(pIdPedido: string): void {
    const dialogRef = this._dialogService.open(GenericConfirmDialogComponent, {
      // width: '600px',
      panelClass: 'custom-dialog-container',
      data: "¿Seguro que quieres cancelar este pedido?",
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
       //Si se pulso en aceptar
          let request = new CancelaPedidoRequest();
          request.IdPedido = pIdPedido;
          this._pedidoService.cancelar(request).subscribe(
            respuesta => {
              if (respuesta.Exito) {
                this.mensajes.showSuccess(respuesta.Mensaje);
                this.getPedidos();
              } else
                this.mensajes.showWarning(respuesta.Mensaje);
            }, error => {
              this.mensajes.showError(error.message);
          });
      }
    });

  }
}

