import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ButtonComponent } from '@components/atoms/button/button.component';
import { ThemeService } from '@services/core/theme.service';
import { AuthenticationService } from '@services/authentication/authentication.service';
import { UserSigninRequest } from '@models/dtos/authentication-dtos';

@Component({
  selector: 'app-signin-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ButtonComponent],
  templateUrl: './signin-form.component.html',
})
export class SigninFormComponent implements OnInit {
  loginForm!: FormGroup;
  isSubmitting = false;

  constructor(public themeService: ThemeService, private fb: FormBuilder, private authenticationService: AuthenticationService) { }

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

      return; // Empêche la soumission si le formulaire est invalide
    }

    this.isSubmitting = true;
    console.log('Form Data:', this.loginForm.value);

    let signinResquest: UserSigninRequest = {
      email: this.email?.value,
      password: this.password?.value,
    }

    this.authenticationService.signin(signinResquest).subscribe(rep => {
      console.log(rep);
      this.isSubmitting = false;
    });
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }
}