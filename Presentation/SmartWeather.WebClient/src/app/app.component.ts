import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';  // Import Router for navigation
import { AuthService } from '@services/core/auth.service'; // Import AuthService to check authentication status

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'SmartWeather';

  constructor() { }

}
