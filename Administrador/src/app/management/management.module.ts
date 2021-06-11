import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManagementRoutingModule } from './management-routing.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

/**Componentes externos npm */
import { ToastrModule } from 'ngx-toastr';
import { SidebarModule } from 'ng-sidebar';
import { NgxLoadingModule, ngxLoadingAnimationTypes } from 'ngx-loading';

import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { MaterialFileInputModule } from 'ngx-material-file-input';
//Servicios
import { AppMaterialModule } from '../app-material/app-material.module';
import { AgmCoreModule } from '@agm/core';

//Directivas
import { DisableControlDirective } from '../core/directives/DisableControlDirective';
import { OnlyNumber } from '../core/directives/OnlyNumbers';

//Pipes
import { ThousandsPipe } from '../core/pipes/decimal.pipe';

//Componentes
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NotfoundComponent } from './components/notfound/notfound.component';
import { HomeComponent } from './components/home/home.component';
import { LocalComponent } from './components/local/local.component';
import { LocalAddComponent } from './components/local-add/local-add.component';
import { ProductoComponent } from './components/producto/producto.component';
import { ProductoAddComponent } from './components/producto-add/producto-add.component';
import { RepartidorComponent } from './components/repartidor/repartidor.component';
import { RepartidorAddComponent } from './components/repartidor-add/repartidor-add.component';
import { MenuComponent } from './components/menu/menu.component';
import { HeaderComponent } from './components/header/header.component';
import { PedidosComponent } from './components/pedidos/pedidos.component';
import { ProductoExtrasModalAeComponent } from './components/producto-extras-modal-ae/producto-extras-modal-ae.component';
import { PedidosViewIdComponent } from './components/pedidos-view-id/pedidos-view-id.component';
import { RepartidorMensajeEnviarComponent } from './components/repartidor-mensaje-enviar/repartidor-mensaje-enviar.component';
import { BalanceComponent } from './components/balance/balance.component';
import { UsuariosComponent } from './components/usuarios/usuarios.component';
import { UsuariosAddComponent } from './components/usuarios-add/usuarios-add.component';
import { BannersComponent } from './components/banners/banners.component';
import { TipoLocalComponent } from './components/tipo-local/tipo-local.component';

@NgModule({
  declarations: [
    DashboardComponent,
    MenuComponent,
    HeaderComponent,
    NotfoundComponent,
    HomeComponent,
    LocalComponent,
    ProductoComponent,
    LocalAddComponent,
    ProductoAddComponent,
    DisableControlDirective,
    OnlyNumber,
    ThousandsPipe,
    // ConductorAddComponent,
    RepartidorComponent,
    RepartidorAddComponent,
    PedidosComponent,
    ProductoExtrasModalAeComponent,
    PedidosViewIdComponent,
    RepartidorMensajeEnviarComponent,
    BalanceComponent,
    UsuariosComponent,
    UsuariosAddComponent,
    BannersComponent,
    TipoLocalComponent,],
  imports: [
    CommonModule,
    ManagementRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    AppMaterialModule,
    //Servicio para alertas
    ToastrModule.forRoot({
      positionClass: 'toast-top-right',
      preventDuplicates: true,
      progressBar: true
    }),
    //Componente de menú lateral
    SidebarModule.forRoot(),
    //Block loading
    NgxLoadingModule.forRoot({
      animationType: ngxLoadingAnimationTypes.threeBounce,
      backdropBackgroundColour: 'rgba(0,0,0,0.2)',
      backdropBorderRadius: '4px',
      primaryColour: '#bb1934',
      secondaryColour: '#07415a',
      tertiaryColour: '#fff',
      fullScreenBackdrop: true
    }),
    //Mapas
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyAx_MQ3PvBlWz1a42tCO_ZNTGK5w3EbXbY',
      libraries: ['places']
    }),
    NgxMatSelectSearchModule,
    MaterialFileInputModule,
  ],entryComponents : [
    ProductoExtrasModalAeComponent
  ]
})
export class ManagementModule { }
