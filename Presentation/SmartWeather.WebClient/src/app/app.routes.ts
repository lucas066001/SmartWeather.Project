import { Routes } from '@angular/router';
import { LoginPageComponent } from '@pages/login/login.component';
import { RegisterPageComponent } from '@pages/register/register.component';

export const routes: Routes = [
    { path: 'login', component: LoginPageComponent },
    { path: 'register', component: RegisterPageComponent },
];
