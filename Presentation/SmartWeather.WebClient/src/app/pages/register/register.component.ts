import { Component } from '@angular/core';
import { RegisterFormComponent } from '@components/organisms/auth/register-form/register-form.component';
import { AuthenticationTemplateComponent } from '@templates/authentication-template/authentication-template.component';

@Component({
  selector: 'app-register',
  imports: [AuthenticationTemplateComponent, RegisterFormComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterPageComponent {

}
