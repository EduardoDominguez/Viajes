import { Component, OnInit, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

import { AlertService } from 'src/app/core/services/alert.service';

import { merge, Subscription } from 'rxjs';
import { tap, debounceTime } from 'rxjs/operators';
import { globals } from 'src/app/core/globals/globals';
import { FormControl, FormGroup } from '@angular/forms';

// import { CentroCostoAddDialogComponent } from '../centro-costo-add-dialog/centro-costo-add-dialog.component';
// import { CentroCostoEditDialogComponent } from '../centro-costo-edit-dialog/centro-costo-edit-dialog.component';
// import { CentroCostoConsultaDialogComponent } from '../centro-costo-consulta-dialog/centro-costo-consulta-dialog.component';
// import { CentroCostoActualizarRequest } from 'src/app/classes/request/plazas/unidades-organizacionales/CentroCostoActualizarRequest';
// import { AuthService } from 'src/app/core/services/auth.service';
import { RptGeneralDataSource } from 'src/app/core/datasources/rpt-general-datasource';
import { ReportesService } from 'src/app/core/services/reportes.service';
import { RptGanancia } from 'src/app/classes/RptGanancia';
// import { TipoDependencia } from 'src/app/classes/plazas/unidades-organizacionales/TipoDependencia';
// import { UnidadAdministrativa } from 'src/app/classes/plazas/unidades-organizacionales/UnidadAdministrativa';
// import { UnidadAdministrativaService } from 'src/app/core/services/plazas/unidades-organizacionales/unidad-administrativa.service';
// import { TipoDependenciaService } from 'src/app/core/services/plazas/unidades-organizacionales/tipo-dependencia.service';
// import { EstatusRegistro } from 'src/app/classes/enum/EstatusRegistro';
// import { Grid } from 'src/app/classes/abstract/Grid';
// import { NivelJerarquicoEnum } from 'src/app/classes/enum/NivelJerarquicoEnum';
var dateFormat = require("dateformat");

@Component({
  selector: 'app-reporte-general',
  templateUrl: './reporte-general.component.html',
  styleUrls: ['./reporte-general.component.scss']
})
export class ReporteGeneralComponent implements OnInit, OnDestroy, AfterViewInit {

  //Seccion variables
  displayedColumns: string[] = ['folio', 'fechaPedido', 'nombreCliente', 'emailCliente', 'direccionEntrega', 'nombreLocal', 'nombreResponsableLocal', 'detallePedido', 'nombreTipoLocal', 'nombreMetodoPago', 'costoTotal', 'totalLocal', 'costoViaje', 'nombreRepartidor'];
  dataSource: RptGeneralDataSource;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  termino_busqueda: FormControl = new FormControl("");


  //Subscribers
  private subscriptionConsultaTipoDependencia: Subscription;
  private obsDatasource: Subscription;
  private subscriptionConsultaUA: Subscription;

  //Listas filtros
  // public listTipoDependencia: TipoDependencia[];
  // public listUnidadAdministrativa: UnidadAdministrativa[];


  //Valores seleccionados combo
  public cmbTipoDependnecia = new FormControl(null);
  public cmbUnidadAdministrativa =  new FormControl(null);



 // Forms
  public rangoFechaCreacion = new FormGroup({
    start: new FormControl(),
    end: new FormControl()
  });

  constructor(
    private _mensajesService: AlertService,
    private _reportesService: ReportesService,
    public _globalesService: globals,
    public _dialogService: MatDialog,
  ) {
    // super();
  }

  ngOnInit() {
    this.dataSource = new RptGeneralDataSource(this._reportesService);

    setTimeout(() => {
      this.dataSource.loadRptGeneral(0, 10, 'fechaPedido', 'asc', this.termino_busqueda.value);
      // this.getListaTipoDependencia();
    });

    this.termino_busqueda.valueChanges.pipe(
      debounceTime(500)
    ).subscribe(value => {
      this.paginator.pageIndex = 0;
      this.paginator.pageSize = 10;
      this.reLoadGridPage()
    });
  }

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => {
      this.paginator.pageIndex = 0;
    });

    this.obsDatasource = merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap(() => this.reLoadGridPage())
      )
      .subscribe();
  }


  ngOnDestroy() {
    if (this.obsDatasource)
      this.obsDatasource.unsubscribe();
  }

  /**
   * Limpia los filtros de la pantalla y refresca los datos
   */
  clearFilter(): void {
    this.termino_busqueda.setValue("");
    this.rangoFechaCreacion.reset();
    // this.cmbTipoDependnecia.setValue(null);
    // this.cmbUnidadAdministrativa.setValue(null);
    // this.listUnidadAdministrativa = new Array();
    // this.reLoadGridPage();
  }

  /**
   * buscar con filtro
   */
  public buscarPorFiltro(): void {
    // console.log(this.rangoFechaCreacion);
    if (this.rangoFechaCreacion.invalid
      || (this.rangoFechaCreacion.controls['start'].value != null && this.rangoFechaCreacion.controls['end'].value == null)
      || (this.rangoFechaCreacion.controls['start'].value == null && this.rangoFechaCreacion.controls['end'].value != null))
      this._mensajesService.showWarning("Debe ingresar una fecha inicio y fin correctas para la busqueda");
    else
      this.reLoadGridPage();

  }

  /**
   * Exporta grid a excle
   */
  exportDataExcel(): void {
    // this._reportesSerice.getExcel(
    //   0,//Para que no pagine
    //   0,
    //   (!this.sort.active) ? 'dependencia' : this.sort.active,
    //   (!this.sort.direction) ? 'asc' : this.sort.direction,
    //   this.termino_busqueda.value,
    //   (this.cmbTipoDependnecia.value == null) ? null : this.cmbTipoDependnecia.value.id,
    //   (this.cmbUnidadAdministrativa.value == null) ? null : this.cmbUnidadAdministrativa.value.id).subscribe(
    //   (respuesta: any) => {
    //     if (respuesta.size && respuesta.size > 0)
    //       this._globalesService.downloadFile(respuesta, "Centros de costo.xlsx", respuesta.type);
    //     else
    //       this._mensajesService.showWarning("No se pudo generar el archivo excel, intente más tarde.")
    //   },
    //   error => {
    //   }
    // );
  }

  /**
   * Refresca el grid de datos
   */
  reLoadGridPage(): void {
    this.dataSource.loadRptGeneral(
      this.paginator.pageIndex,
      this.paginator.pageSize,
      (!this.sort.active) ? 'fechaPedido' : this.sort.active,
      (!this.sort.direction) ? 'asc' : this.sort.direction,
      this.termino_busqueda.value,
      (this.rangoFechaCreacion.invalid && !this.rangoFechaCreacion.controls["start"].value) ? '' : dateFormat(this.rangoFechaCreacion.controls["start"].value, "yyyy-mm-dd"),
      (this.rangoFechaCreacion.invalid && !this.rangoFechaCreacion.controls["end"].value) ? '' : dateFormat(this.rangoFechaCreacion.controls["end"].value, "yyyy-mm-dd")
    );
  }

}


