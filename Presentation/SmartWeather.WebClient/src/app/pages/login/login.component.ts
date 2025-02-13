import { Component } from '@angular/core';
import { SigninFormComponent } from '@components/organisms/signin-form/signin-form.component';
import { AuthenticationTemplateComponent } from '@templates/authentication-template/authentication-template.component';

@Component({
  selector: 'app-login',
  imports: [AuthenticationTemplateComponent, SigninFormComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginPageComponent {
  pageTitle = 'Signin';
}
