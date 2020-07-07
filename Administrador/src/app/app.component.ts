import { Component, OnInit, HostListener } from '@angular/core';
import { Router, NavigationEnd } from "@angular/router";

import { StorageService } from "./core/services/storage.service";
import { LoadingService } from './core/services/loading.service';
import { Sesion } from './classes/Sesion';
import { MenuService } from './core/services/menu.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {


  constructor(

  ) {
  }

}
