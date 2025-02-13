import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ButtonComponent } from '@components/atoms/button/button.component';
import { Status } from '@constants/api-status';
import { ApiResponse } from '@models/api-response';
import { UserRegisterRequest, UserSigninRequest, UserSigninResponse } from '@models/dtos/authentication-dtos';
import { AuthenticationService } from '@services/authentication/authentication.service';
import { AuthService } from '@services/core/auth.service';
import { ThemeService } from '@services/core/theme.service';

@Component({
  selector: 'app-register-form',
  imports: [ButtonComponent, ReactiveFormsModule, CommonModule],
  templateUrl: './register-form.component.html',
  styleUrl: './register-form.component.css'
})
export class RegisterFormComponent implements OnInit {
  registerForm!: FormGroup;
  isSubmitting = false;
  incorrectInputs = false;

  constructor(public themeService: ThemeService, private fb: FormBuilder, private authService: AuthService, private authenticationService: AuthenticationService, private router: Router) { }

  ngOnInit() {
    this.registerForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        username: ['', [Validators.required, Validators.minLength(3)]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]]
      },
      { validators: this.passwordMatchValidator() }
    );
  }

  submitForm() {
    console.log(this.registerForm);
    console.log('Form Data:', this.registerForm.value);

    if (this.registerForm.invalid) {
      console.log('Formulaire invalide');
      this.incorrectInputs = true;
      return;
    }

    this.isSubmitting = true;
    console.log('Form Data:', this.registerForm.value);

    let registerResquest: UserRegisterRequest = {
      name: this.username?.value,
      email: this.email?.value,
      password: this.password?.value,
    }

    this.authenticationService.register(registerResquest).subscribe(({
      next: (response) => {
        console.log('Création réussie :', response);
        if (response.status == Status.OK && response.data) {
          this.authService.setToken(response.data.token);
          this.router.navigate(['/dashboard']);
        }
        this.isSubmitting = false;
      },
      error: (error: ApiResponse<UserSigninResponse>) => {
        console.log('Erreur de connexion :', error);
        this.incorrectInputs = true;
        this.isSubmitting = false;
      }
    }));

  }

  passwordMatchValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const password = control.get('password')?.value;
      const confirmPassword = control.get('confirmPassword')?.value;

      return password && confirmPassword && password !== confirmPassword
        ? { passwordMismatch: true }
        : null;
    };
  }

  resetError() {
    this.incorrectInputs = false;
    console.log('resetError');
  }

  get username() {
    return this.registerForm.get('username');
  }

  get email() {
    return this.registerForm.get('email');
  }

  get password() {
    return this.registerForm.get('password');
  }

  get confirmPassword() {
    return this.registerForm.get('confirmPassword');
  }
}
