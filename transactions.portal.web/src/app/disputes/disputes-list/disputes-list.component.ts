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
  templateUrl: './disputes-list.component.html',
  styleUrl: './disputes-list.component.scss'
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
