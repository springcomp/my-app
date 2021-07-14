import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { Err404Component } from './err404/err404.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { MeComponent } from './me/me.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/home' },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'me', component: MeComponent },
  { path: '404', component: Err404Component },
  { path: '**', redirectTo: '/404' }
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
