import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ModalModule } from 'ngx-bootstrap/modal';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxMaskModule } from 'ngx-mask';
import { NgxCurrencyModule } from 'ngx-currency';
import { ToastrModule } from 'ngx-toastr';
import { DateTimeFormatPipePipe } from './_helps/DateTimeFormatPipe.pipe';

import { EventoService } from './_services/evento.service';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { EventosComponent } from './eventos/eventos.component';
import { EventoEditComponent } from './eventos/eventoEdit/eventoEdit.component';
import { PalestrantesComponent } from './palestrantes/palestrantes.component';
import { ContatosComponent } from './contatos/contatos.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TituloComponent } from './_shared/titulo/titulo.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { AuthInterceptor } from './auth/auth.interceptor';

@NgModule({
   declarations: [
      AppComponent,
      DateTimeFormatPipePipe,
      NavComponent,
      EventosComponent,
      EventoEditComponent,
      PalestrantesComponent,
      ContatosComponent,
      DashboardComponent,
      TituloComponent,
      UserComponent,
      LoginComponent,
      RegistrationComponent
   ],
   imports: [
      BrowserModule,
      TooltipModule.forRoot(),
      BsDropdownModule.forRoot(),
      ModalModule.forRoot(),
      BsDatepickerModule.forRoot(),
      TabsModule.forRoot(),
      NgxMaskModule.forRoot(),
      NgxCurrencyModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      BrowserAnimationsModule,
      ReactiveFormsModule,
      ToastrModule.forRoot({
        timeOut: 3000,
        preventDuplicates: true,
        progressBar: true
     }),
    ],
    providers: [
      EventoService,
      {
         provide: HTTP_INTERCEPTORS,
         useClass: AuthInterceptor,
         multi: true
      }
    ],
    bootstrap: [
       AppComponent
    ]
 })
 export class AppModule { }
