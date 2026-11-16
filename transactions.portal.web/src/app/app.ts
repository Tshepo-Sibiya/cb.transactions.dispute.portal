import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ErrorBannerComponent } from './shared/components/error-banner/error-banner.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive, ErrorBannerComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
}
