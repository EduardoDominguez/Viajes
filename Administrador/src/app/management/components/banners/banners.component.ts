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
import { ActualizaEstatusGenericoRequest } from 'src/app/classes/request/ActualizaEstatusGenericoRequest';
import { BannerService } from 'src/app/core/services/banner.service';
import { Banner } from 'src/app/classes/Banner';
import { MatDialog } from '@angular/material/dialog';
import { FormBannerComponent } from 'src/app/management/modals/form-banner/form-banner.component';
import { pid } from 'process';


@AutoUnsubscribe()
@Component({
  selector: 'app-banners',
  templateUrl: './banners.component.html',
  styleUrls: ['./banners.component.scss']
})
export class BannersComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['acciones', 'estatus', 'Fotografia', 'Nombre' ];
  dataSource: any;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  idLocal: number = 0;
  nombreLocal: string = "";
  constructor(
    private _bannersService: BannerService,
    private mensajes: AlertService,
    private storageService: StorageService,
    public globales: globals,
    private _router: Router,
    private _route: ActivatedRoute,
    private _location: Location,
    public _dialogService: MatDialog

  ) {
    this.procesaRutas();
  }

  public ngOnDestroy() { }

  ngOnInit() {
    setTimeout(() => {
      this.getBanners();
    });

  }

  /**
   * Detecta los parametros de la ruta
   */
  procesaRutas() {
    this.idLocal = +this._route.snapshot.paramMap.get('idLocal');
    this._route.queryParamMap.subscribe(queryParams => {
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
  onChangeEstatus(pElement: any, pId: string) {
    let request = new ActualizaEstatusGenericoRequest<string>(pId, pElement.checked ? 1 : 0, this.storageService.getCurrentSession().Persona.IdPersona);
    this._bannersService.actualizaEstatus(request).subscribe(
      respuesta => {
        if (respuesta.Exito) {
          this.getBanners();

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
   * Consume servicio para consultar usuarios.
   */
  public getBanners(): void {
    this._bannersService.getBanners().subscribe(
      respuesta => {
        // console.log(respuesta)
        if (respuesta.Exito) {
          this.dataSource = new MatTableDataSource<Banner>(respuesta.Data);
          this.initOptionsMattable();
        } else
          this.mensajes.showWarning(respuesta.Mensaje);
      }, error => {
        this.mensajes.showError(error.message);
      });
  }


 /**
   * Abre el panel para agregar un banner
   */
  public agregar(): void {
    const dialogRef = this._dialogService.open(FormBannerComponent, {
      // width: '600px',
      panelClass: 'custom-dialog-container',
      data: "¿Seguro que quieres cancelar este pedido?",
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getBanners();
      }
    });
  }


  /**
   * Abre el panel para consultar un banner
   */
   public consultar(pIdBanner: string): void {
    const dialogRef = this._dialogService.open(FormBannerComponent, {
      // width: '600px',
      panelClass: 'custom-dialog-container',
      // data: "¿Seguro que quieres cancelar este pedido?",
    });

    this._router.navigate([], {
      relativeTo: this._route,
      queryParams: {
        id: pIdBanner,
        to: 'c'
      },
      queryParamsHandling: 'merge',
      // preserve the existing query params in the route
      skipLocationChange: true
      // do not trigger navigation
    });

    // dialogRef.afterClosed().subscribe(result => {
    //   if (result) {
    //     this.getBanners();
    //   }
    // });
  }


  /**
   * Abre el panel para editar un banner
   */
   public editar(pIdBanner: string): void {
    const dialogRef = this._dialogService.open(FormBannerComponent, {
      // width: '600px',
      panelClass: 'custom-dialog-container',
      // data: "¿Seguro que quieres cancelar este pedido?",
    });

    this._router.navigate([], {
      relativeTo: this._route,
      queryParams: {
        id: pIdBanner,
        to: 'e'
      },
      queryParamsHandling: 'merge',
      // preserve the existing query params in the route
      skipLocationChange: true
      // do not trigger navigation
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getBanners();
      }
    });
  }



}

