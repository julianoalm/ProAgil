import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EventosComponent } from './eventos/eventos.component';
import { ContatosComponent } from './contatos/contatos.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { PalestrantesComponent } from './palestrantes/palestrantes.component';

//Caso digite na QueryString eventos, abra o componente EventosComponent e assim por diante.
//No app.component.html nas tags <router-outlet></router-outlet> é o lugar onde será aberto a rota abaixo.
const routes: Routes = [
  { path: 'eventos', component:EventosComponent }
  , { path: 'palestrantes', component:PalestrantesComponent }
  , { path: 'dashboard', component:DashboardComponent }
  , { path: 'contatos', component:ContatosComponent }
  , { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
  , { path: '**', redirectTo: 'dashboard', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
