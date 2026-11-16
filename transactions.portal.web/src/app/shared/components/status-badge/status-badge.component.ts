import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [CommonModule],
  template: `
    <span class="status-text" [ngClass]="badgeClass">
      {{ label }}
    </span>
  `,
  styles: [`
    .status-text {
      display: inline-block;
      font-size: inherit;
      font-weight: 500;
      line-height: inherit;
      white-space: nowrap;
      background: transparent;
      padding: 0;
    }

    .badge-success {
      color: #047857;
    }

    .badge-warning {
      color: #b45309;
    }

    .badge-danger {
      color: #b91c1c;
    }

    .badge-info {
      color: #1d4ed8;
    }

    .badge-default {
      color: #475569;
    }
  `]
})
export class StatusBadgeComponent {
  @Input() status: string = '';

  get label(): string {
    if (this.status === 'UnderReview') {
      return 'Under Review';
    }
    return this.status;
  }

  get badgeClass(): string {
    const s = this.status?.toLowerCase();
    switch (s) {
      case 'completed':
      case 'resolved':
        return 'badge-success';
      case 'disputed':
      case 'open':
      case 'underreview':
        return 'badge-warning';
      case 'refunded':
        return 'badge-info';
      case 'rejected':
        return 'badge-danger';
      default:
        return 'badge-default';
    }
  }
}

