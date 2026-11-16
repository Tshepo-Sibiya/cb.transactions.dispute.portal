import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  readonly error = signal<string | null>(null);
  readonly success = signal<string | null>(null);

  showError(message: string): void {
    this.error.set(message);
  }

  clearError(): void {
    this.error.set(null);
  }

  showSuccess(message: string): void {
    this.success.set(message);
    setTimeout(() => {
      this.clearSuccess();
    }, 5000);
  }

  clearSuccess(): void {
    this.success.set(null);
  }
}

