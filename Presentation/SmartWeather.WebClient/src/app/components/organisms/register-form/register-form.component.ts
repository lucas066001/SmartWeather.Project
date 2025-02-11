import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ButtonComponent } from '@components/atoms/button/button.component';
import { ApiResponse } from '@models/api-response';
import { UserRegisterRequest, UserSigninRequest, UserSigninResponse } from '@models/dtos/authentication-dtos';
import { AuthenticationService } from '@services/authentication/authentication.service';
import { ThemeService } from '@services/core/theme.service';

@Component({
  selector: 'app-register-form',
  imports: [ButtonComponent, ReactiveFormsModule, CommonModule],
  templateUrl: './register-form.component.html',
  styleUrl: './register-form.component.css'
})
export class RegisterFormComponent {
  registerForm!: FormGroup;
  isSubmitting = false;
  incorrectInputs = false;

  constructor(public themeService: ThemeService, private fb: FormBuilder, private authenticationService: AuthenticationService, private router: Router) { }

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
        this.router.navigate(['/dashboard']);
      },
      error: (error: ApiResponse<UserSigninResponse>) => {
        console.log('Erreur de connexion :', error);
        this.incorrectInputs = true;
      }
    }));

    this.isSubmitting = false;
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
