import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';

import { Router } from '@angular/router';
//import {AuthenticationService} from "../../services/login/authentication.service";
import { StorageService } from "../../../core/services/storage.service";
import { AlertService } from "../../../core/services/alert.service";
import { LoadingService } from '../../../core/services/loading.service';
import { globals } from '../../../core/globals/globals';
import { MenuService } from 'src/app/core/services/menu.service';
import { AuthenticationService } from 'src/app/core/services/authentication.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  // Usamos el decorador Output
  //@Output() CambiaMenu = new EventEmitter();

  public _menuToggler: boolean;
  //private _tipoFlecha : string;
  public nombreSistema: string;
  public nombrePersona: string;

  constructor(
    private authenticationService: AuthenticationService,
    public storageService: StorageService,
    private mensajes: AlertService,
    public router: Router,
    private globales: globals,
    private _menuService : MenuService,
  ) { }

  ngOnInit() {
    //this._tipoFlecha = 'fas fa-angle-double-right';
    this.nombreSistema = this.globales.SYSTEM.NAME;
    this.nombrePersona = this.storageService.getCurrentSession().Persona.Nombre;
  }

  // Cuando se lance el evento click en la plantilla llamaremos a este método
  toggleMenu(event) {
    //this._menuToggler = !this._menuToggler;
    // Usamos el método emit
    //this.CambiaMenu.emit({ menuToggler: this._menuToggler });
    this._menuService.toggle();
  }

  /*cambioContrasena():void{
    this.loadingService.toggle();
    let parametros = {pProceso: 'CAMBIANDO', pUsuario: this.storageService.getCurrentUser().username, pPassword: 'N/A', pToken: '', pUrl: ''};
    this.authenticationService.recuperarPassword(parametros).subscribe(
      data  => {
        if(data.estatus){
          //this.mensajes.showSuccess(data.mensaje);
          this.router.navigate(['/cambio-contrasena/'+data.data]);
        }
        else
          this.mensajes.showWarning(data.mensaje);
    }, error => {
        this.mensajes.showError(error.name);
    }, () =>{
        this.loadingService.toggle();
    });
  }*/

  onLoggedout() {
    // this.authenticationService.logout().subscribe(
    //   data => {
        this.storageService.logout();
      // }, error => {
      //   this.mensajes.showError(error.message);
      // });
  }

}
