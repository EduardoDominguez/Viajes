import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';

import { Router } from '@angular/router';
//import {AuthenticationService} from "../../services/login/authentication.service";
import { StorageService } from "../../services/storage.service";
import { AlertService } from "../../services/alert.service";
import { LoadingService } from '../../services/loading.service';
import { globals } from '../../globals/globals';
import { MenuService } from 'src/app/services/menu.service';

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
  constructor(
    //private authenticationService: AuthenticationService,
    public storageService: StorageService,
    //private mensajes: AlertService,
    public router: Router,
    private globales: globals,
    private _menuService : MenuService,
  ) { }

  ngOnInit() {
    //this._tipoFlecha = 'fas fa-angle-double-right';
    this.nombreSistema = this.globales.SYSTEM.NAME;
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
    /*this.authenticationService.logout().subscribe(
      data => {
        this.storageService.logout();
      }, error => {
        this.mensajes.showError(error.message);
      });*/
  }

}
