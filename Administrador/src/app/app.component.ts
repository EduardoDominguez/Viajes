import { Component, OnInit, HostListener } from '@angular/core';
import { Router, NavigationEnd } from "@angular/router";

import { StorageService } from "./services/storage.service";
import { LoadingService } from './services/loading.service';
import { Sesion } from './classes/Sesion';
import { MenuService } from './services/menu.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  //indica si la resolición es posiblemente de un móvil
  public _isMobile : boolean;

  public loading: boolean;
  public _opened: boolean = false;
  public _animate: boolean = false;
  public _closeOnClickOutSide: boolean;
  public _mode: string;
  public _dock: boolean;
  public _dockedSize: string;
  //public _colapsarMenu :boolean = false; //Controla la vista del menú  
  public _showBackdrop: boolean;
  /**Para mostrar o no cotenido en la página inicio */
  public _rutaActiva: boolean;

  sesion: Sesion;

  constructor(
    private _storageService: StorageService,
    private router: Router,
    private loadingService: LoadingService,
    private _menuService: MenuService,
  ) {

    this.sesion = new Sesion();
    this.sesion.token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ik1hcmlvIEVkdWFyZG8gRG9taW5ndWV6IE1lbGVuZGV6IiwibmJmIjoxNTcwNTA0ODU2LCJleHAiOjE1NzkxNDQ4NTYsImlhdCI6MTU3MDUwNDg1NiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MTE1NyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTExNTcifQ.PTYvSwtqnxUK3iaMxIUSyzTgP519FAC2X06-2lgWrsk";
    this._storageService.setCurrentSession(this.sesion);

    router.events.subscribe((val) => {
      if (val instanceof NavigationEnd) {
        if (this._storageService.getCurrentSession() == null)
          this._storageService.logout();

        this._rutaActiva = router.url === '/';
      }

    });

    //console.log(this.storageService.getCurrentSession());
    this.loading = false;
    //this._autoCollapseOnInit = false;
    this._closeOnClickOutSide = true;

    if (window.matchMedia("(max-width: 700px)").matches) {
      this._isMobile = true;
      /* The viewport is less than, or equal to, 700 pixels wide */
      this._dockedSize = '0';
      this._mode = 'over';
      this._showBackdrop = true;
      this._dock = false;
    } else {
      /* The viewport is greater than 700 pixels wide */
      this._isMobile = false;
      this._dockedSize = '50px';
      this._mode = 'push';
      this._dock = true;
    }

    //console.log(this.storageService.getCurrentSession());
  }


  ngOnInit(): void {


    //Hardcodeado solo para ejemplo

    //this._storageService.setCurrentSession(new Sesion());

    /*setTimeout(() => {
      if(this.cookieService.check('_opened')){
        this._opened = this.cookieService.get('_opened').toLowerCase() == 'true' ? true : false;
        //this._colapsarMenu = this._opened;
      }
    });*/

    //setTimeout(() => {
    this.loadingService.change.subscribe((isOpen: boolean) => {
      //console.log(isOpen);
      this.loading = isOpen;
    });

    this._menuService.change.subscribe((isOpened: boolean) => {
      this._opened = isOpened;
      //console.log(this._opened);
    });
    //});
  }

  validaSesionActiva(): boolean {
    return this._storageService.isAuthenticated();
  }

  /*_toggleSidebar(mode, menu) {
    menu.toggleCollapsed(this._opened);
    this._opened = mode.menuToggler;
  }
*/
  _onBackdropClicked() {
    this._menuService.toggle();
  }

  onClosed(event) {
    if (this._isMobile) {
      /* The viewport is less than, or equal to, 700 pixels wide */
    } else {
      /* The viewport is greater than 700 pixels wide */
      document.getElementsByTagName("aside")[0].removeAttribute("style");
    }
  }
}
