import { NgModule, LOCALE_ID } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
// import { CommonModule } from '@angular/common';

import { globals } from './globals/globals';
import { AlertService } from "./services/alert.service";
import { ComunService } from './services/comun.service';
import { LoadingService } from './services/loading.service';
import { StorageService } from './services/storage.service';
import { TitleService } from './services/title.service';
import { MenuService } from './services/menu.service';

// import { AuthService } from './services/auth.service';
// import { AuthModule, OidcSecurityService } from 'angular-auth-oidc-client';
import { AuthGuardService } from './services/auth-guard.service';
//Servicio para cambios en menu
// import { ToggleMenuService } from "./services/toggle-menu.service";
// import { NgxPermissionsModule } from 'ngx-permissions';
import { AuthInterceptorService } from './services/auth-interceptor.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

/**Componentes externos npm */
// import { ToastrModule } from 'ngx-toastr';
// import { SidebarModule } from 'ng-sidebar';
// import { NgxLoadingModule, ngxLoadingAnimationTypes } from 'ngx-loading';

/*Para la parte de idiomas. */
import { registerLocaleData } from '@angular/common';
import localeMX from '@angular/common/locales/es-MX';
import { PaginatorEspañol } from '../locales/spanish-paginator-intl';
import { MatPaginatorIntl } from '@angular/material/paginator';

//Directives
// import { OnlyNumber } from './directives/OnlyNumbers';
// import { DisableControlDirective } from './directives/DisableControlDirective';


registerLocaleData(localeMX, 'es-MX');


@NgModule({
  imports: [
    HttpClientModule,
    // AuthModule.forRoot(),
  ],
  declarations: [
    // OnlyNumber,
    // DisableControlDirective,
  ],
  providers: [
    globals,
    AlertService,
    ComunService,
    LoadingService,
    StorageService,
    TitleService,
    MenuService,
    // ToggleMenuService,
    AuthGuardService,
    {
      provide: LOCALE_ID,
      useValue: 'es-MX'
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true,
    },
    { provide: MatPaginatorIntl, useClass: PaginatorEspañol },

  ],
  exports: []
})
export class CoreModule {
  constructor(titleService: TitleService) {
    titleService.init();
  }
}
