import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Location } from '@angular/common';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

import { AlertService } from "../../../core/services/alert.service";
import { StorageService } from 'src/app/core/services/storage.service';
import { globals } from '../../../core/globals/globals';
import { ProductoService } from 'src/app/core/services/producto.service';
import { Producto } from 'src/app/classes/Producto';
import { ActivatedRoute, Router } from '@angular/router';
import { AutoUnsubscribe } from "ngx-auto-unsubscribe";

@AutoUnsubscribe()
@Component({
  selector: 'app-producto',
  templateUrl: './producto.component.html',
  styleUrls: ['./producto.component.scss']
})
export class ProductoComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['acciones', 'estatus', 'IdProducto', 'Nombre', 'Descripcion', 'Precio'];
  dataSource: any;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  idLocal: number = 0;
  nombreLocal: string = "";
  constructor(
    private _productoService: ProductoService,
    private mensajes: AlertService,
    private _storageService: StorageService,
    public globales: globals,
    private router: Router,
    private route: ActivatedRoute,
    private _location: Location
  ) {
    this.procesaRutas();
  }

  public ngOnDestroy() {}

  ngOnInit() {
    // setTimeout(() => {
    //   this.getProductos();
    // });

    setTimeout(() => {
      if (this.idLocal > 0)
        this.getProductosByIdLocal(this.idLocal);

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
    //let request = new ActualizaEstatusGenericoRequest(pId, pElement.checked, this.storageService.getCurrentSession().user.idpersona, 'nodo');
    let request = new Producto();
    request.IdProducto = pId;
    request.Estatus = pElement.checked ? 1 : 0;
    request.IdPersonaModifica = this._storageService.getCurrentUser().IdPersona;
    this._productoService.actualizaEstatus(request).subscribe(
      respuesta => {
        if (respuesta.Exito) {
          this.getProductosByIdLocal(this.idLocal);
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
  private getProductos(): void {
    this._productoService.getProductos().subscribe(
      respuesta => {
        // console.log(respuesta)
        if (respuesta.Exito) {
          this.dataSource = new MatTableDataSource<Producto>(respuesta.Data);
          this.initOptionsMattable();
        } else
          this.mensajes.showWarning(respuesta.Mensaje);
      }, error => {
        this.mensajes.showError(error.message);
      });
  }

  /**
   * Consume servicio para consultar locales por local.
   * @param pIdLocal
   */
  private getProductosByIdLocal(pIdLocal: number): void {
    this._productoService.getProductoByIdLocal(pIdLocal).subscribe(
      respuesta => {
        console.log(respuesta)
        if (respuesta.Exito) {
          this.dataSource = new MatTableDataSource<Producto>(respuesta.Data);
          this.initOptionsMattable();
        } else
          this.mensajes.showWarning(respuesta.Mensaje);
      }, error => {
        this.mensajes.showError(error.message);
      });
  }
}

