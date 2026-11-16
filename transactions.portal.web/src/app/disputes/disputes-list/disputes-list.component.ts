import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { DisputeDetail, DisputeListItem } from '../../core/models/dispute.model';
import { DisputesService } from '../../core/services/disputes.service';
import { StatusBadgeComponent } from '../../shared/components/status-badge/status-badge.component';

@Component({
  selector: 'app-disputes-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, StatusBadgeComponent],
  template: `
    <div class="page-container">
      <header class="page-header">
        <div>
          <h1 class="page-title">Dispute History</h1>
          <p class="page-description">
            Track and inspect the status and progress timeline of filed dispute cases.
          </p>
        </div>
      </header>

      <section class="filters-card">
        <div class="filters-grid">
          <div class="filter-item">
            <label for="status" class="filter-label">Status</label>
            <select
              id="status"
              class="filter-select"
              [(ngModel)]="status"
              (change)="loadDisputes()">
              <option value="">All Statuses</option>
              <option value="Open">Open</option>
              <option value="UnderReview">Under Review</option>
              <option value="Resolved">Resolved</option>
              <option value="Rejected">Rejected</option>
            </select>
          </div>

          <div class="filter-item">
            <label for="fromDate" class="filter-label">From Date</label>
            <input
              id="fromDate"
              type="date"
              class="filter-input"
              [(ngModel)]="fromDate"
              (change)="loadDisputes()" />
          </div>

          <div class="filter-item">
            <label for="toDate" class="filter-label">To Date</label>
            <input
              id="toDate"
              type="date"
              class="filter-input"
              [(ngModel)]="toDate"
              (change)="loadDisputes()" />
          </div>

          <div class="filter-actions">
            <button type="button" class="btn btn-secondary" (click)="resetFilters()">
              Reset
            </button>
            <button type="button" class="btn btn-primary" (click)="loadDisputes()">
              Filter
            </button>
          </div>
        </div>
      </section>

      @if (loading()) {
        <div class="state-text">Loading dispute history...</div>
      } @else if (disputes().length > 0) {
        <div class="table-container">
          <table class="data-table">
            <thead>
              <tr>
                <th>Status</th>
                <th>Transaction Reference</th>
                <th>Merchant</th>
                <th class="text-right">Amount</th>
                <th>Date Opened</th>
                <th>Date Resolved</th>
                <th class="text-right">Timeline</th>
              </tr>
            </thead>
            <tbody>
              @for (item of disputes(); track item.id) {
                <tr
                  class="clickable-row"
                  [class.row-selected]="selectedDisputeId() === item.id"
                  (click)="toggleDispute(item.id)">
                  <td>
                    <app-status-badge [status]="item.status" />
                  </td>
                  <td>{{ item.transactionReference }}</td>
                  <td>{{ item.merchant }}</td>
                  <td class="text-right">
                    {{ item.amount | currency:'ZAR':'symbol-narrow':'1.2-2' }}
                  </td>
                  <td>{{ item.createdAt | date:'yyyy-MM-dd HH:mm' }}</td>
                  <td>
                    {{ item.resolvedAt ? (item.resolvedAt | date:'yyyy-MM-dd HH:mm') : '&mdash;' }}
                  </td>
                  <td class="text-right text-action">
                    {{ selectedDisputeId() === item.id ? 'Hide &uarr;' : 'View &rarr;' }}
                  </td>
                </tr>

                @if (selectedDisputeId() === item.id) {
                  <tr class="details-row">
                    <td colspan="7">
                      <div class="timeline-container">
                        @if (loadingDetail()) {
                          <div class="timeline-loading">Loading status timeline...</div>
                        } @else if (selectedDispute(); as d) {
                          <div class="timeline-header">
                            <div>
                              <h3 class="timeline-title">Dispute Timeline: {{ d.reason }}</h3>
                              <p class="timeline-desc">{{ d.description }}</p>
                            </div>
                            <a [routerLink]="['/transactions', d.transactionId]" class="btn btn-link">
                              View Original Transaction &rarr;
                            </a>
                          </div>

                          <div class="timeline-steps">
                            @for (step of d.statusHistory; track step.id; let last = $last) {
                              <div class="timeline-step">
                                <div class="step-marker" [class.step-last]="last"></div>
                                <div class="step-content">
                                  <div class="step-header">
                                    <app-status-badge [status]="step.status" />
                                    <span class="step-time">
                                      {{ step.changedAt | date:'yyyy-MM-dd HH:mm:ss' }}
                                    </span>
                                  </div>
                                  @if (step.note) {
                                    <div class="step-note">{{ step.note }}</div>
                                  }
                                </div>
                              </div>
                            }
                          </div>
                        }
                      </div>
                    </td>
                  </tr>
                }
              }
            </tbody>
          </table>
        </div>
      } @else {
        <div class="state-text">No disputes found matching your criteria.</div>
      }
    </div>
  `,
  styles: [`
    .page-container {
      max-width: 1100px;
      margin: 0 auto;
      padding: 1.5rem 1rem;
    }

    .page-header {
      margin-bottom: 1.25rem;
    }

    .page-title {
      font-size: 1.5rem;
      font-weight: 600;
      color: #0a2540;
      margin: 0 0 0.25rem 0;
    }

    .page-description {
      color: #64748b;
      font-size: 0.875rem;
      margin: 0;
    }

    .filters-card {
      background: #ffffff;
      border: 1px solid #e2e8f0;
      border-radius: 4px;
      padding: 1rem;
      margin-bottom: 1.5rem;
    }

    .filters-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
      gap: 0.75rem;
      align-items: flex-end;
    }

    .filter-item {
      display: flex;
      flex-direction: column;
      gap: 0.25rem;
    }

    .filter-label {
      font-size: 0.75rem;
      font-weight: 600;
      color: #475569;
      text-transform: uppercase;
      letter-spacing: 0.05em;
    }

    .filter-input,
    .filter-select {
      border: 1px solid #cbd5e1;
      border-radius: 4px;
      padding: 0.5rem 0.625rem;
      font-size: 0.875rem;
      color: #1e293b;
      background-color: #ffffff;
      box-sizing: border-box;
      font-family: inherit;
    }

    .filter-input:focus,
    .filter-select:focus {
      outline: none;
      border-color: #0a2540;
    }

    .filter-actions {
      display: flex;
      gap: 0.5rem;
    }

    .btn {
      padding: 0.5rem 1rem;
      font-size: 0.875rem;
      font-weight: 500;
      border-radius: 4px;
      cursor: pointer;
      border: 1px solid transparent;
      text-decoration: none;
    }

    .btn-secondary {
      background-color: #ffffff;
      color: #475569;
      border-color: #cbd5e1;
    }

    .btn-secondary:hover {
      background-color: #f8fafc;
    }

    .btn-primary {
      background-color: #0a2540;
      color: #ffffff;
    }

    .btn-primary:hover {
      background-color: #10365c;
    }

    .btn-link {
      background: none;
      border: none;
      color: #0a2540;
      padding: 0;
      font-size: 0.8125rem;
    }

    .btn-link:hover {
      text-decoration: underline;
    }

    .table-container {
      background: #ffffff;
      border: 1px solid #e2e8f0;
      border-radius: 4px;
      overflow-x: auto;
    }

    .data-table {
      width: 100%;
      border-collapse: collapse;
      text-align: left;
      font-size: 0.875rem;
    }

    .data-table th {
      background-color: #f8fafc;
      color: #475569;
      font-weight: 600;
      padding: 0.75rem 1rem;
      border-bottom: 1px solid #e2e8f0;
      white-space: nowrap;
    }

    .data-table td {
      padding: 0.75rem 1rem;
      border-bottom: 1px solid #f1f5f9;
      vertical-align: middle;
      color: #1e293b;
      font-size: 0.875rem;
      font-weight: 400;
    }

    .clickable-row {
      cursor: pointer;
    }

    .clickable-row:hover {
      background-color: #f8fafc;
    }

    .row-selected {
      background-color: #f1f5f9 !important;
    }

    .text-right {
      text-align: right;
    }

    .text-action {
      color: #0a2540;
      font-size: 0.8125rem;
    }

    .details-row td {
      background-color: #f8fafc;
      padding: 1.25rem 1.5rem;
      border-bottom: 2px solid #e2e8f0;
    }

    .timeline-container {
      display: flex;
      flex-direction: column;
      gap: 1rem;
    }

    .timeline-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      border-bottom: 1px solid #e2e8f0;
      padding-bottom: 0.75rem;
      gap: 1rem;
    }

    .timeline-title {
      font-size: 1rem;
      font-weight: 600;
      color: #0a2540;
      margin: 0 0 0.25rem 0;
    }

    .timeline-desc {
      font-size: 0.875rem;
      color: #475569;
      margin: 0;
    }

    .timeline-steps {
      display: flex;
      flex-direction: column;
      gap: 1rem;
      padding-left: 0.5rem;
      position: relative;
    }

    .timeline-step {
      display: flex;
      gap: 1rem;
      position: relative;
    }

    .step-marker {
      width: 10px;
      height: 10px;
      border-radius: 50%;
      background-color: #0a2540;
      margin-top: 0.35rem;
      position: relative;
      flex-shrink: 0;
    }

    .step-marker::after {
      content: '';
      position: absolute;
      top: 10px;
      left: 4px;
      width: 2px;
      height: calc(100% + 2rem);
      background-color: #cbd5e1;
    }

    .step-last::after {
      display: none;
    }

    .step-content {
      display: flex;
      flex-direction: column;
      gap: 0.25rem;
      flex: 1;
    }

    .step-header {
      display: flex;
      align-items: center;
      gap: 0.75rem;
    }

    .step-time {
      font-size: 0.75rem;
      color: #64748b;
      font-family: ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, monospace;
    }

    .step-note {
      font-size: 0.875rem;
      color: #334155;
    }

    .timeline-loading {
      color: #64748b;
      font-size: 0.875rem;
      padding: 0.5rem 0;
    }

    .state-text {
      padding: 3rem 0;
      text-align: center;
      color: #64748b;
      font-size: 0.95rem;
    }
  `]
})
export class DisputesListComponent implements OnInit {
  private readonly disputesService = inject(DisputesService);

  status: string = '';
  fromDate: string = '';
  toDate: string = '';

  readonly disputes = signal<DisputeListItem[]>([]);
  readonly loading = signal<boolean>(true);

  readonly selectedDisputeId = signal<string | null>(null);
  readonly selectedDispute = signal<DisputeDetail | null>(null);
  readonly loadingDetail = signal<boolean>(false);

  ngOnInit(): void {
    this.loadDisputes();
  }

  loadDisputes(): void {
    this.loading.set(true);
    this.disputesService.getDisputes(this.status, this.fromDate, this.toDate).subscribe({
      next: data => {
        this.disputes.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }

  resetFilters(): void {
    this.status = '';
    this.fromDate = '';
    this.toDate = '';
    this.loadDisputes();
  }

  toggleDispute(id: string): void {
    if (this.selectedDisputeId() === id) {
      this.selectedDisputeId.set(null);
      this.selectedDispute.set(null);
      return;
    }

    this.selectedDisputeId.set(id);
    this.loadingDetail.set(true);

    this.disputesService.getDisputeById(id).subscribe({
      next: detail => {
        this.selectedDispute.set(detail);
        this.loadingDetail.set(false);
      },
      error: () => {
        this.loadingDetail.set(false);
      }
    });
  }
}
