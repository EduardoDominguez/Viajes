import { Component, OnInit, OnDestroy, AfterViewInit } from "@angular/core";
import { Router } from "@angular/router";

import { LoginRequest } from "../classes/request/LoginRequest";
import { AuthenticationService } from "../core/services/authentication.service";
import { StorageService } from "../core/services/storage.service";
import { AlertService } from "../core/services/alert.service";
import { trigger, state, style, animate, transition } from '@angular/animations';

import { Sesion } from "../classes/Sesion";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  animations: [
    trigger('fadeIn', [
      state('void', style({
        opacity: 0
      })),
      transition('void <=> *', animate(800)),
    ]),
  ]
})
export class LoginComponent implements OnInit, OnDestroy {
  public loading = false;

  usuario: string;
  password: string;
  objSuscribe: any;

  constructor(
    private authenticationService: AuthenticationService,
    private storageService: StorageService,
    private router: Router,
    private mensajes: AlertService,
  ) { }

  ngOnInit() {
    this.usuario = "";
    this.password = "";

    if (this.storageService.isAuthenticated())
      this.storageService.removeCurrentSession();
  }

  ngOnDestroy() {
    if (this.objSuscribe != null)
      this.objSuscribe.unsubscribe();
  }

  validar():boolean {

    if(this.usuario.trim() == ''){
      this.mensajes.showWarning("Debes capturar tu correo.")
      return false;
    }

    if(this.password.trim() == ''){
      this.mensajes.showWarning("Debes capturar la contraseña.")
      return false;
    }

    return true;
  }

  iniciarSesion() {

    if(!this.validar())
      return;

    this.loading = true;
    let login = new LoginRequest({ Email: this.usuario, Password: this.password, TipoUsuario: 1 });
    this.objSuscribe = this.authenticationService.login(login).subscribe(
      data => {
        // console.log(data)
        if (data.Exito)
          this.correctLogin(data);
        else
          this.mensajes.showWarning(data.Mensaje);

        this.loading = false;
      }, error => {
        //console.log(error);
        this.mensajes.showError(error.name);
        this.loading = false;
      });
  }

  /*public submitLogin(): void {
    this.submitted = true;
    this.error = null;
    if(this.loginForm.valid){
      this.authenticationService.login(new LoginObject(this.loginForm.value)).subscribe(
        data => this.correctLogin(data),
        error => this.error = JSON.parse(error._body)
      )
    }
  }*/

  private correctLogin(data: Sesion) {
    this.storageService.setCurrentSession(data);
    // this.router.navigate(['/dashboard/home']);
    this.router.navigate(['/admin-dashboard']);

  }
}
