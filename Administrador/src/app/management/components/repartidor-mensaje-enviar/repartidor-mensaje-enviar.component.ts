import { Component, OnInit, ViewChild, OnDestroy, } from '@angular/core';
import { FormControl, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { AlertService } from "../../../core/services/alert.service";
import { globals } from '../../../core/globals/globals';
import { ActivatedRoute, Router } from '@angular/router';
import { AutoUnsubscribe } from "ngx-auto-unsubscribe";
import { Repartidor } from 'src/app/classes/Repartidor';
import { PersonaService } from 'src/app/core/services/persona.service';
import { MatSelect } from '@angular/material/select';
import { Subject } from 'rxjs';
import { EnviarNotificacionFirebaseRequest } from 'src/app/classes/request/EnviarNotifiacionFirebaseRequest';

@AutoUnsubscribe()
@Component({
  selector: 'app-repartidor-mensaje-enviar',
  templateUrl: './repartidor-mensaje-enviar.component.html',
  styleUrls: ['./repartidor-mensaje-enviar.component.scss']
})
export class RepartidorMensajeEnviarComponent implements OnInit, OnDestroy {

  public listaRepartidores = new Array<Repartidor>();
  form: FormGroup;


  constructor(
    private _repartidorService: PersonaService,
    private _mensajes: AlertService,
    public globales: globals,
    private fb: FormBuilder,

  ) { }

  ngOnInit(): void {
    setTimeout(() => {
      this.getRepartidores();
    });

     this.form = this.fb.group({
      cmbRepartidor: [
        null,
        [Validators.required]
      ],
      txtMensaje: [
        null,
        [Validators.required, Validators.maxLength(150)]
      ]
    });
  }

  public ngOnDestroy() { }


  /**
   * Consume servicio para consultar repartidores.
   */
  public getRepartidores(): void {
    this._repartidorService.getRepartidores().subscribe(
      respuesta => {
        console.log(respuesta)
        if (respuesta.Exito) {
          this.listaRepartidores = respuesta.Data;
        } else
          this._mensajes.showWarning(respuesta.Mensaje);
      }, error => {
        this._mensajes.showError(error.message);
      });
  }

  public enviarMensaje(): void{
    if (this.form.invalid) {
      this._mensajes.showWarning('Ingrese todos los datos obligatorios');
    }else{
        let request = new EnviarNotificacionFirebaseRequest();
        request.Titulo = "MENSAJE ADMINISTRADOR";
        request.Mensaje = this.form.controls['txtMensaje'].value.trim();
        request.TipoNotificacion = 2;//2 es para conductor
        request.IdPersona = this.form.controls['cmbRepartidor'].value;

        this._repartidorService.enviarNotificacionFirebase(request).subscribe(
          respuesta => {
            if (respuesta.Exito) {
              this._mensajes.showSuccess("Mensaje enviado");
              this.form.reset();
              //this.limpiaCamposFormulario();
            } else {
              this._mensajes.showWarning("Algo salió mal al enviar el mensaje, intente más tarde.");
            }

          }, error => {
            this._mensajes.showError(error.message);
          });
    }
  }
}
