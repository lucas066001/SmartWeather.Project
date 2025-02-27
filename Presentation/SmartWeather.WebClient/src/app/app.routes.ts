import { Routes } from '@angular/router';
import { ConfiguratorPageComponent } from '@pages/configurator/configurator.component';
import { DashboardPageComponent } from '@pages/dashboard/dashboard.component';
import { LoginPageComponent } from '@pages/login/login.component';
import { RegisterPageComponent } from '@pages/register/register.component';
import { WateringPageComponent } from '@pages/watering/watering.component';

export const routes: Routes = [
    { path: 'login', component: LoginPageComponent },
    { path: 'register', component: RegisterPageComponent },
    { path: 'dashboard', component: DashboardPageComponent },
    { path: 'config', component: ConfiguratorPageComponent },
    { path: 'config/:id', component: ConfiguratorPageComponent },
    { path: 'watering', component: WateringPageComponent },
    { path: 'watering/:id', component: WateringPageComponent },
    { path: '', redirectTo: 'login', pathMatch: 'full' },
];
