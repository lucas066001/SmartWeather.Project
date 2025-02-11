import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';  // Import Router for navigation
import { AuthService } from '@services/core/auth.service'; // Import AuthService to check authentication status

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'SmartWeather';

  constructor(
    private router: Router,
    private authService: AuthService
  ) { }

  // OnInit lifecycle hook to perform the redirect logic
  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/dashboard']);
    } else {
      this.router.navigate(['/login']);
    }
  }
}
