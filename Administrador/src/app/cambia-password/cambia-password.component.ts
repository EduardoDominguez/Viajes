import { Component, OnInit, OnDestroy } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";

import { ActualizaPasswordRequest } from "../classes/request/ActualizaPasswordRequest";
import { StorageService } from "../core/services/storage.service";
import { AlertService } from "../core/services/alert.service";
import { trigger, state, style, animate, transition } from '@angular/animations';
import { PersonaService } from "../core/services/persona.service";
import { FormControl } from "@angular/forms";
import { AuthenticationService } from "../core/services/authentication.service";


@Component({
  selector: 'app-cambia-password',
  templateUrl: './cambia-password.component.html',
  styleUrls: ['./cambia-password.component.scss'],
  animations: [
    trigger('fadeIn', [
      state('void', style({
        opacity: 0
      })),
      transition('void <=> *', animate(800)),
    ]),
  ]
})
export class CambiaPasswordComponent implements OnInit, OnDestroy {

  public loading = false;
  public seCambio = false;
  public tokenValido = false;

  mensajeToken: string = "Cargando ...";
  idPersona: number;
  tokenPassword: string;
  objSuscribe: any;

  txtPassword = new FormControl("");
  txtConfirmaPassword = new FormControl("");

  constructor(
    private _autentifiacionService: AuthenticationService,
    private storageService: StorageService,
    private router: Router,
    private route: ActivatedRoute,
    private mensajes: AlertService,
  ) {
    this.procesaParametrosURL();
  }

  procesaParametrosURL() {
    this.idPersona = +this.route.snapshot.paramMap.get('idPersona');
    this.tokenPassword = this.route.snapshot.paramMap.get('token');

    if(!this.idPersona || !this.tokenPassword)
      this.router.navigate([``], { relativeTo: this.route });

    // this.route.queryParamMap.subscribe(queryParams => {
    //   this.tipoOperacion = (queryParams.get("to") == null) ? "n" : queryParams.get("to").toLocaleLowerCase();
    //   this.isDisabled = this.tipoOperacion == ('c' || 'n');
    //   this.detectaTipoOperacion(this.tipoOperacion);
    // });
  }

  ngOnInit() {
    if (this.storageService.isAuthenticated())
      this.storageService.removeCurrentSession();

    //Valida si la url sigue siendo válida
    this.validaTokenValido();
  }

  ngOnDestroy() {
    if (this.objSuscribe != null)
      this.objSuscribe.unsubscribe();
  }


  validaTokenValido():void{
    this.objSuscribe = this._autentifiacionService.validaToken(this.idPersona, this.tokenPassword).subscribe(
      data => {
        // console.log(data)
        if (data.Exito){
          // this.mensajes.showSuccess(data.Mensaje);
          this.tokenValido = true;
        }
        else{
          // this.mensajes.showWarning(data.Mensaje);
          this.mensajeToken = data.Mensaje;
        }


      }, error => {
        //console.log(error);
        this.mensajes.showError(error.name);
        this.loading = false;
      });
  }

  validar():boolean {

    if(this.txtPassword.value.trim() == ''){
      this.mensajes.showWarning("Debes capturar tu nueva contraseña.")
      return false;
    }

    if(this.txtConfirmaPassword.value.trim() == ''){
      this.mensajes.showWarning("Debes confirmar la contraseña.")
      return false;
    }

    if(this.txtConfirmaPassword.value.trim() != this.txtPassword.value.trim()){
      this.mensajes.showWarning("Las contraseñas no coinciden, favor de revisarlo.")
      return false;
    }

    return true;
  }

  iniciarSesion() {

    if(!this.validar())
      return;

    this.loading = true;
    let request = new ActualizaPasswordRequest();
    request.IdPersona = this.idPersona;
    request.IdTipoPersona = 2;
    request.TokenPassword = this.tokenPassword;
    request.Password = this.txtPassword.value.trim();

    this.objSuscribe = this._autentifiacionService.actualizaPassword(request).subscribe(
      data => {
        // console.log(data)
        if (data.Exito){
          this.mensajes.showSuccess(data.Mensaje);
          this.seCambio = true;
        }
        else
          this.mensajes.showWarning(data.Mensaje);

        this.loading = false;
      }, error => {
        //console.log(error);
        this.mensajes.showError(error.name);
        this.loading = false;
      });
  }

  public cerrar(): void{
    window.top.close();
  }
}
