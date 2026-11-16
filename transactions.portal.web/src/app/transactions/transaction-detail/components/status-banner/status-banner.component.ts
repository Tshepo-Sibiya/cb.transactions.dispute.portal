import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-transaction-status-banner',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './status-banner.component.html',
  styleUrl: './status-banner.component.scss'
})
export class StatusBannerComponent {
  @Input() status: string = '';

  get statusLabel(): string {
    return this.status || 'Transaction';
  }

  get statusMessage(): string {
    const s = this.status?.toLowerCase();
    switch (s) {
      case 'completed':
        return 'This transaction has settled successfully and is eligible for review.';
      case 'disputed':
        return 'A dispute is active for this transaction. Case review is in progress.';
      case 'refunded':
        return 'A refund has been credited back to your original payment card.';
      default:
        return 'Transaction details are displayed below.';
    }
  }

  get bannerClass(): string {
    const s = this.status?.toLowerCase();
    switch (s) {
      case 'completed':
        return 'banner-completed';
      case 'disputed':
        return 'banner-disputed';
      case 'refunded':
        return 'banner-refunded';
      default:
        return 'banner-default';
    }
  }
}
