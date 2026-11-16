import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { PagedResult } from '../../core/models/paged-result.model';
import { TransactionListItem } from '../../core/models/transaction.model';
import { TransactionsService } from '../../core/services/transactions.service';
import { StatusBadgeComponent } from '../../shared/components/status-badge/status-badge.component';

@Component({
  selector: 'app-transactions-list',
  standalone: true,
  imports: [CommonModule, FormsModule, StatusBadgeComponent],
  templateUrl: './transactions-list.component.html',
  styleUrl: './transactions-list.component.scss'
})
export class TransactionsListComponent implements OnInit {
  private readonly transactionsService = inject(TransactionsService);
  private readonly router = inject(Router);

  protected readonly Math = Math;

  search: string = '';
  status: string = '';
  fromDate: string = '';
  toDate: string = '';
  currentPage: number = 1;
  pageSize: number = 10;

  readonly data = signal<PagedResult<TransactionListItem> | null>(null);
  readonly loading = signal<boolean>(true);

  ngOnInit(): void {
    this.loadTransactions();
  }

  applyFilters(): void {
    this.currentPage = 1;
    this.loadTransactions();
  }

  resetFilters(): void {
    this.search = '';
    this.status = '';
    this.fromDate = '';
    this.toDate = '';
    this.currentPage = 1;
    this.loadTransactions();
  }

  changePage(newPage: number): void {
    this.currentPage = newPage;
    this.loadTransactions();
  }

  viewDetails(id: string): void {
    this.router.navigate(['/transactions', id]);
  }

  startDispute(id: string): void {
    this.router.navigate(['/transactions', id, 'dispute']);
  }

  private loadTransactions(): void {
    this.loading.set(true);
    this.transactionsService.getTransactions(
      this.search,
      this.status,
      this.fromDate,
      this.toDate,
      this.currentPage,
      this.pageSize
    ).subscribe({
      next: result => {
        this.data.set(result);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }
}
