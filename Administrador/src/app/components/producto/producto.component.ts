import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

import { AlertService } from "../../services/alert.service";
import { StorageService } from 'src/app/services/storage.service';
import { globals } from '../../globals/globals';
import { ProductoService } from 'src/app/services/producto.service';
import { Producto } from 'src/app/classes/Producto';



@Component({
  selector: 'app-producto',
  templateUrl: './producto.component.html',
  styleUrls: ['./producto.component.scss']
})
export class ProductoComponent implements OnInit {
  displayedColumns: string[] = [ 'acciones', 'estatus', 'IdProducto', 'Nombre', 'Descripcion', 'Precio'];
  dataSource : any;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  constructor(
    private _productoService: ProductoService,
    private mensajes: AlertService,
    private storageService: StorageService,
    public globales: globals,
     ) { }

  ngOnInit() {
    setTimeout(() => {
      this.getProductos();
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
    let request = new Producto();
    request.IdProducto = pId;
    request.Estatus = pElement.checked ? 1 : 0;
    request.IdPersonaModifica = 1//this.storageService.getCurrentSession().user.idpersona;
    this._productoService.actualizaEstatus(request).subscribe(
      respuesta => {
        if (respuesta.Exito) {
          this.getProductos()
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
  private getProductos() :void {
    this._productoService.getProductos().subscribe(
      respuesta => {
        console.log(respuesta)
        if (respuesta.Exito){
          this.dataSource = new MatTableDataSource<Producto>(respuesta.Data);
          this.initOptionsMattable();
        }else
          this.mensajes.showWarning(respuesta.Mensaje);
      }, error => {
        this.mensajes.showError(error.message);
      });
  }
}
