import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonComponent } from '@components/atoms/button/button.component';
import { Status } from '@constants/api/api-status';
import { ApiResponse } from '@models/api-response';
import { StationClaimRequest, StationResponse } from '@models/dtos/station-dtos';
import { StationService } from '@services/station/station.service';

@Component({
  selector: 'app-station-claim-tool',
  imports: [ButtonComponent, CommonModule, ReactiveFormsModule],
  templateUrl: './station-claim-tool.component.html',
  styleUrl: './station-claim-tool.component.css'
})
export class StationClaimToolComponent implements OnInit {
  @Output() newStationClaimedEvent: EventEmitter<StationResponse> = new EventEmitter<StationResponse>();

  isSubmitting: boolean = false;
  claimError: boolean = false;
  errorMessage: string = "Unknown error";
  claimForm!: FormGroup;

  constructor(private fb: FormBuilder, private stationService: StationService) { }

  ngOnInit(): void {
    this.claimForm = this.fb.group({
      macaddress: ['', [Validators.required, Validators.minLength(10)]]
    });
  }

  resetError() {
    this.claimError = false;
  }

  submitForm() {
    console.log(this.claimForm);
    console.log('Form Data:', this.claimForm.value);

    if (this.claimForm.invalid) {
      console.log('Formulaire invalide');
      this.claimError = true;
      return;
    }

    this.isSubmitting = true;

    let claimResquest: StationClaimRequest = {
      macAddress: this.macaddress?.value
    }

    this.stationService.claim(claimResquest).subscribe(({
      next: (response) => {
        console.log('Connexion réussie :', response);
        if (response.status == Status.OK && response.data) {
          this.newStationClaimedEvent.emit(response.data);
        } else {
          this.claimError = true;
          this.errorMessage = response.message;
        }
        this.isSubmitting = false;
      },
      error: (response: any) => {
        console.log('Erreur de connexion :', response);
        this.claimError = true;
        this.errorMessage = response.error.message;
        this.isSubmitting = false;
      }
    }));
  }

  get macaddress() {
    return this.claimForm.get('macaddress');
  }
}
