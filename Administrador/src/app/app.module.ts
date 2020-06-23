import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';

// /**Componentes externos npm */
// import { ToastrModule } from 'ngx-toastr';
// import { SidebarModule } from 'ng-sidebar';
// import { NgxLoadingModule, ngxLoadingAnimationTypes } from 'ngx-loading';

//Angular material
// import {
//   MatTableModule, MatInputModule, MatPaginatorModule, MatSortModule, MatProgressSpinnerModule,
//   MatFormFieldModule, MatSelectModule, MatToolbarModule, MatIconModule, MatButtonModule,
//   MatTooltipModule, MatTabsModule, MatMenuModule, MatPaginatorIntl, MatSlideToggleModule
// // } from '@angular/material';
// import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
// import { MaterialFileInputModule } from 'ngx-material-file-input';

/**Componentes */
import { AppComponent } from './app.component';
// import { MenuComponent } from './management/components/menu/menu.component';
// import { HeaderComponent } from './management/components/header/header.component';
// import { NotfoundComponent } from './management/components/notfound/notfound.component';
// import { HomeComponent } from './management/components/home/home.component';
// import { LocalComponent } from './management/components/local/local.component';
// import { LocalAddComponent } from './management/components/local-add/local-add.component';
// import { ProductoComponent } from './management/components/producto/producto.component';
// import { ProductoAddComponent } from './management/components/producto-add/producto-add.component';

/*Directivas */
// import { DisableControlDirective } from './core/directives/DisableControlDirective';

/**Animaciones */
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

/**Modulos */
import { CoreModule }  from './core/core.module';
import { ManagementModule }  from './management/management.module';

// /**Servicios */
// import { globals } from './core/globals/globals';
// import { TitleService } from './core/services/title.service';
// //Servicio para loading
// import { LoadingService } from './core/services/loading.service';
// //Servicios para toast
// import { AlertService } from "./core/services/alert.service";
// //Servicios para el menu
// import { MenuService } from './core/services/menu.service';

//Angular material
import { AppMaterialModule } from './app-material/app-material.module';
import { LoginComponent } from './login/login.component';

// import { AuthInterceptorService } from './core/services/auth-interceptor.service';
// import { HTTP_INTERCEPTORS } from '@angular/common/http';
// import { ConductorAddComponent } from './components/conductor-add/conductor-add.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
  ],
  imports: [
    BrowserModule,
    ManagementModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    CoreModule,
    AppMaterialModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}

//ng build --base-href / --prod --configuration=production  --output-hashing none

