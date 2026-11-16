import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-error-banner',
  standalone: true,
  imports: [CommonModule],
  template: `
    @if (notificationService.error(); as error) {
      <div class="banner banner-error">
        <span class="banner-message">{{ error }}</span>
        <button type="button" class="banner-close" (click)="notificationService.clearError()" aria-label="Dismiss">
          &times;
        </button>
      </div>
    }

    @if (notificationService.success(); as success) {
      <div class="banner banner-success">
        <span class="banner-message">{{ success }}</span>
        <button type="button" class="banner-close" (click)="notificationService.clearSuccess()" aria-label="Dismiss">
          &times;
        </button>
      </div>
    }
  `,
  styles: [`
    .banner {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 0.75rem 1rem;
      border-radius: 4px;
      margin-bottom: 1rem;
      font-size: 0.875rem;
    }

    .banner-error {
      background-color: #fde8e8;
      border: 1px solid #f8b4b4;
      color: #9b1c1c;
    }

    .banner-success {
      background-color: #def7ec;
      border: 1px solid #bcf0da;
      color: #03543f;
    }

    .banner-message {
      font-weight: 500;
    }

    .banner-close {
      background: none;
      border: none;
      font-size: 1.25rem;
      line-height: 1;
      cursor: pointer;
      color: inherit;
      padding: 0 0.25rem;
    }
  `]
})
export class ErrorBannerComponent {
  readonly notificationService = inject(NotificationService);
}

