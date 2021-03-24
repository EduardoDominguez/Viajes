import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CambiaPasswordComponent } from './cambia-password/cambia-password.component';
import { LoginComponent } from './login/login.component';


const routes: Routes = [
  { path: '', component: LoginComponent, pathMatch:'full' },
  { path: 'cambia-password/:idPersona/:token', component: CambiaPasswordComponent },
  { path: 'admin-dashboard', loadChildren: () => import(`./management/management.module`).then(m => m.ManagementModule) },
  // { path: '', redirectTo: '/home', pathMatch:'full' },
  // { path: 'home', component: HomeComponent, pathMatch:'full' },
  // { path: 'administracion/locales', component: LocalComponent },
  // { path: 'administracion/locales/agregar', component: LocalAddComponent },
  // { path: 'administracion/locales/:id', component: LocalAddComponent },
  // { path: 'administracion/productos', component: ProductoComponent },
  // { path: 'administracion/productos/agregar', component: ProductoAddComponent },
  // { path: 'administracion/productos/:id', component: ProductoAddComponent },
  // { path: 'administracion/repartidor', component: RepartidorComponent },
  // { path: 'administracion/repartidor/agregar', component: RepartidorAddComponent },
  // { path: 'no-encontrado',  component: NotfoundComponent},
  { path: '**', redirectTo: '', pathMatch: 'full'},

];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true,  /*enableTracing: true,*/ scrollPositionRestoration: 'enabled', } )],
  exports: [RouterModule]
})
export class AppRoutingModule { }
