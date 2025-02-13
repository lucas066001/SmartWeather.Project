import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ButtonComponent } from '@components/atoms/button/button.component';
import { ThemeService } from '@services/core/theme.service';
import { AuthenticationService } from '@services/authentication/authentication.service';
import { UserSigninRequest, UserSigninResponse } from '@models/dtos/authentication-dtos';
import { ApiResponse } from '@models/api-response';
import { Router } from '@angular/router';
import { Status } from '@constants/api-status';
import { AuthService } from '@services/core/auth.service';

@Component({
  selector: 'app-signin-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ButtonComponent],
  templateUrl: './signin-form.component.html',
})
export class SigninFormComponent implements OnInit {
  loginForm!: FormGroup;
  isSubmitting = false;
  incorrectCredentials = false;

  constructor(public themeService: ThemeService, private fb: FormBuilder, private authService: AuthService, private authenticationService: AuthenticationService, private router: Router) { }

  ngOnInit() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],  // Email requis et format email valide
      password: ['', [Validators.required, Validators.minLength(6)]], // Min 6 caractères
      rememberMe: [false] // Par défaut décoché
    });
  }

  submitForm() {
    console.log(this.loginForm);
    console.log('Form Data:', this.loginForm.value);

    if (this.loginForm.invalid) {
      console.log('Formulaire invalide');
      this.incorrectCredentials = true;
      return;
    }

    this.isSubmitting = true;
    console.log('Form Data:', this.loginForm.value);

    let signinResquest: UserSigninRequest = {
      email: this.email?.value,
      password: this.password?.value,
    }

    this.authenticationService.signin(signinResquest).subscribe(({
      next: (response) => {
        console.log('Connexion réussie :', response);
        if (response.status == Status.OK && response.data) {
          this.authService.setToken(response.data.token);
          this.router.navigate(['/dashboard']);
        }
        this.isSubmitting = false;
      },
      error: (error: ApiResponse<UserSigninResponse>) => {
        console.log('Erreur de connexion :', error);
        this.incorrectCredentials = true;
        this.isSubmitting = false;

      }
    }));

  }

  resetError() {
    this.incorrectCredentials = false;
    console.log('resetError');
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }
}