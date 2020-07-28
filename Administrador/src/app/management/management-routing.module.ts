import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
// import { AdminComponent } from './admin.component';

import { NotfoundComponent } from './components/notfound/notfound.component';
import { HomeComponent } from './components/home/home.component';
import { LocalComponent } from './components/local/local.component';
import { LocalAddComponent } from './components/local-add/local-add.component';
import { ProductoComponent } from './components/producto/producto.component';
import { ProductoAddComponent } from './components/producto-add/producto-add.component';
import { RepartidorComponent } from './components/repartidor/repartidor.component';
import { RepartidorAddComponent } from './components/repartidor-add/repartidor-add.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AuthGuardService } from '../core/services/auth-guard.service';
import { PedidosComponent } from './components/pedidos/pedidos.component';
import { PedidosViewIdComponent } from './components/pedidos-view-id/pedidos-view-id.component';


const routesChild: Routes = [
  {
    path: 'admin-dashboard', component: DashboardComponent, canActivate: [AuthGuardService],
    children: [
      { path: '', redirectTo: 'home', pathMatch: 'full',  canActivate: [AuthGuardService]},
      { path: 'home', component: HomeComponent, pathMatch:'full', canActivate: [AuthGuardService] },
      { path: 'administracion/locales', component: LocalComponent, canActivate: [AuthGuardService]},
      { path: 'administracion/locales/agregar', component: LocalAddComponent, canActivate: [AuthGuardService]},
      { path: 'administracion/locales/:id', component: LocalAddComponent, canActivate: [AuthGuardService] },
      { path: 'administracion/locales/:idLocal/productos', component: ProductoComponent, canActivate: [AuthGuardService] },
      // { path: 'administracion/productos', component: ProductoComponent, canActivate: [AuthGuardService] },
      // { path: 'administracion/locales/:id', component: LocalAddComponent, canActivate: [AuthGuardService] },
      { path: 'administracion/productos/agregar', component: ProductoAddComponent, canActivate: [AuthGuardService]},
      { path: 'administracion/productos/:id', component: ProductoAddComponent,  canActivate: [AuthGuardService] },
      { path: 'administracion/repartidor', component: RepartidorComponent, canActivate: [AuthGuardService]},
      { path: 'administracion/repartidor/agregar', component: RepartidorAddComponent, canActivate: [AuthGuardService] },
      { path: 'administracion/repartidor/:id', component: RepartidorAddComponent, canActivate: [AuthGuardService]},
      { path: 'pedidos/consulta', component: PedidosComponent, canActivate: [AuthGuardService] },
      { path: 'pedidos/:id', component: PedidosViewIdComponent,  canActivate: [AuthGuardService] },

      { path: 'no-encontrado', component: NotfoundComponent },
      { path: '**', redirectTo: 'no-encontrado' },
    ]
  }
];


@NgModule({
  imports: [RouterModule.forChild(routesChild)],
  exports: [RouterModule]
})
export class ManagementRoutingModule { }
