// Credits and thanks:
//   https://toddmotto.com/dynamic-page-titles-angular-2-router-events
//   https://stackoverflow.com/questions/34597835/how-to-get-current-route
//   
import { Injectable } from '@angular/core';

import { Title } from '@angular/platform-browser';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { filter, map, switchMap } from 'rxjs/operators';
import { globals } from '../globals/globals';

/**Nombre principal de la aplicación */
var APP_TITLE: string = '';

/**Caracter que separará en el title de la página por secciones */
const SEPARATOR: string = ' > ';

@Injectable()
export class TitleService {

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private titleService: Title,
    private globales: globals,
  ) {
    APP_TITLE = this.globales.SYSTEM.SHORT_NAME;
  }


  init() {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => this.activatedRoute),
      map(route => route.firstChild),
      switchMap(route => route.data),
      map((data) => {
        if (data.title) {
          // If a route has a title set (e.g. data: {title: "Foo"}) then we use it
          return data.title;
        } else {
          // If not, we do a little magic on the url to create an approximation
          return this.router.url.split('/').reduce((acc, frag) => {
            if (acc && frag) { acc += SEPARATOR; }
            return this.router.url.split('/').reduce((acc, frag) => {
              if (acc && frag) { acc += SEPARATOR; }
              return acc + TitleService.ucFirst(frag);
            });
          });
        }
      })
    )
      .subscribe((pathString) => this.titleService.setTitle(`${APP_TITLE} ${pathString}`));
  }

  static ucFirst(string) {
    if (!string) { return string; }
    return string.charAt(0).toUpperCase() + string.slice(1);
  }

}
