import { Routes } from '@angular/router';
import { ConfiguratorPageComponent } from '@pages/configurator/configurator.component';
import { DashboardPageComponent } from '@pages/dashboard/dashboard.component';
import { LoginPageComponent } from '@pages/login/login.component';
import { RegisterPageComponent } from '@pages/register/register.component';

export const routes: Routes = [
    { path: 'login', component: LoginPageComponent },
    { path: 'register', component: RegisterPageComponent },
    { path: 'dashboard', component: DashboardPageComponent },
    { path: 'config', component: ConfiguratorPageComponent },
    { path: 'watering', component: DashboardPageComponent },
    { path: '', redirectTo: 'login', pathMatch: 'full' },
];
