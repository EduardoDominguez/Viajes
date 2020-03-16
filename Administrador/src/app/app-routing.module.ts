import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotfoundComponent } from './components/notfound/notfound.component';
import { HomeComponent } from './components/home/home.component';
import { LocalComponent } from './components/local/local.component';
import { LocalAddComponent } from './components/local-add/local-add.component';
import { ProductoComponent } from './components/producto/producto.component';
import { ProductoAddComponent } from './components/producto-add/producto-add.component';
import { RepartidorComponent } from './components/repartidor/repartidor.component';
import { RepartidorAddComponent } from './components/repartidor-add/repartidor-add.component';


const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch:'full' },
  { path: 'home', component: HomeComponent, pathMatch:'full' },
  { path: 'administracion/locales', component: LocalComponent },
  { path: 'administracion/locales/agregar', component: LocalAddComponent },
  { path: 'administracion/locales/:id', component: LocalAddComponent },
  { path: 'administracion/productos', component: ProductoComponent },
  { path: 'administracion/productos/agregar', component: ProductoAddComponent },
  { path: 'administracion/productos/:id', component: ProductoAddComponent },
  { path: 'administracion/repartidor', component: RepartidorComponent },
  { path: 'administracion/repartidor/agregar', component: RepartidorAddComponent },
  { path: 'no-encontrado',  component: NotfoundComponent},
  { path: '**', redirectTo: '/no-encontrado', pathMatch: 'full'},
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true,  /*enableTracing: true,*/ scrollPositionRestoration: 'enabled', } )],
  exports: [RouterModule]
})
export class AppRoutingModule { }
