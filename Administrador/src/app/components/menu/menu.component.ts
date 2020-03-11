import { Component, Output, EventEmitter, OnInit, Input } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { globals } from '../../globals/globals';
import { MenuService } from 'src/app/services/menu.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

  isActive: boolean;
  collapsed: boolean;
  showMenu: string;
  pushRightClass: string;

  //@Output() collapsedEvent = new EventEmitter<boolean>();

  constructor(
    public globales: globals,
    //private translate: TranslateService,
    public router: Router,
    private _menuService: MenuService,) {
    /*this.translate.addLangs(['en', 'fr', 'ur', 'es', 'it', 'fa', 'de']);
    this.translate.setDefaultLang('en');
    const browserLang = this.translate.getBrowserLang();
    this.translate.use(browserLang.match(/en|fr|ur|es|it|fa|de/) ? browserLang : 'en');*/

    this.router.events.subscribe(val => {

      if ( val instanceof NavigationEnd && window.innerWidth <= 992 && this.isToggled()) {
        this.toggleSidebar();
      }

      if(val instanceof NavigationEnd && window.matchMedia("(max-width: 700px)").matches){
        //console.log("Cambio la ruta")
        this._menuService.toggle(false);
      }
    });
  }

  ngOnInit() {
    this.isActive = false;
    this.collapsed = true;
    this.showMenu = '';
    this.pushRightClass = 'push-right';

    //Captura eventos del menú
    this._menuService.change.subscribe((isOpened : boolean)=>{
      this.collapsed = !isOpened;
      //this.toggleCollapsed(isOpened);
      //console.log(this.collapsed)
    });
  }

  eventCalled() {
    this.isActive = !this.isActive;
  }

  /**
   * Controla que el menú se expanda o se colapse
   * @param element 
   */
  addExpandClass(element: any) :void {
    if (element === this.showMenu) {
      this.showMenu = '0';
    } else {
      this.showMenu = element;
    }
  }

  /*toggleCollapsed(collapsedParam?: boolean): void {
    if (collapsedParam != undefined)
      this.collapsed = collapsedParam;
    else
      this.collapsed = !this.collapsed;
    //this.collapsedEvent.emit(this.collapsed);
    console.log("Desde el menú", this.collapsed)
  }*/

  isToggled(): boolean {
    const dom: Element = document.querySelector('body');
    return dom.classList.contains(this.pushRightClass);
  }

  toggleSidebar() {
    const dom: any = document.querySelector('body');
    dom.classList.toggle(this.pushRightClass);
  }

  /*rltAndLtr() {
      const dom: any = document.querySelector('body');
      dom.classList.toggle('rtl');
  }*/

  /*changeLang(language: string) {
      this.translate.use(language);
  }*/


}
