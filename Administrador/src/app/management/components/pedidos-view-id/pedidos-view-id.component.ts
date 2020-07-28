import { Component, OnInit } from '@angular/core';
import { PedidoService } from 'src/app/core/services/pedido.service';
import { FormControl, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute, GuardsCheckStart } from "@angular/router";
import { AlertService } from "../../../core/services/alert.service";
import { StorageService } from 'src/app/core/services/storage.service';
import { globals } from '../../../core/globals/globals';
import { Pedido } from 'src/app/classes/Pedido';
import { Location } from '@angular/common';

@Component({
  selector: 'app-pedidos-view-id',
  templateUrl: './pedidos-view-id.component.html',
  styleUrls: ['./pedidos-view-id.component.scss']
})
export class PedidosViewIdComponent implements OnInit {

  form: FormGroup;
  idPedido: string;
  pedidoConsultado: Pedido = new Pedido();
  fechaHoraPedido: Date;
  fechaHoraEntrega: Date;

  constructor(
    private route: ActivatedRoute,
    private _alertService: AlertService,
    public globales: globals,
    private fb: FormBuilder,
    private _pedidoService: PedidoService,
    private _location: Location,
  ) {
    this.buildForm();
    this.procesaRutas();
  }

  ngOnInit() {


    setTimeout(() => {
      if (this.idPedido != "")
        this.getPedidoById(this.idPedido);
    });
  }

  goBack(): void {
    this._location.back();
  }

  procesaRutas() {
    this.idPedido = this.route.snapshot.paramMap.get('id');
    this.route.queryParamMap.subscribe(queryParams => {
      // this.tipoOperacion = (queryParams.get("to") == null) ? "n" : queryParams.get("to").toLocaleLowerCase();
      // this.idLocal = (queryParams.get("idLocal") == null) ? 0 : parseInt(queryParams.get("idLocal"));

      // this.isDisabled = this.tipoOperacion == ('c' || 'n');
      // this.detectaTipoOperacion(this.tipoOperacion);
    });
  }

  private buildForm(): void {
    this.form = this.fb.group({
      frmNombre: [
        null,
        [Validators.required, Validators.maxLength(150)]
      ],
      frmDescripcion:
        [
          null,
          [Validators.required, Validators.maxLength(250)]
        ],
      frmPrecio: [
        null,
        [Validators.required,]
      ],
      nodoCtrl: [
        { value: null, disabled: true },
        [Validators.required]
      ],
    });
  }

  private getPedidoById(pIdPedido: string): void {
    this._pedidoService.getPedidoByID(pIdPedido).subscribe(
      respuesta => {
        console.log(respuesta);
        if (respuesta.Exito) {
          this.pedidoConsultado = respuesta.Data;
          // console.log(this.pedidoConsultado.FechaPedido.toString().substring(0, 10) + "T" + this.pedidoConsultado.HoraPedido.toString())
          this.fechaHoraPedido = new Date(this.pedidoConsultado.FechaPedido.toString().substring(0, 10) + "T" + this.pedidoConsultado.HoraPedido.toString());
          var timestamp = Date.parse(this.pedidoConsultado.FechaEntrega.toString());

          if (isNaN(timestamp) == false) {
            this.fechaHoraEntrega = new Date(this.pedidoConsultado.FechaEntrega.toString().substring(0, 10) + "T" + this.pedidoConsultado.HoraEntrega.toString());
          }
        } else
          this._alertService.showWarning(respuesta.Mensaje);
      }, error => {
        this._alertService.showError(error.message);
      });
  }
}
