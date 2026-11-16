import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { TransactionDetail } from '../../core/models/transaction.model';
import { TransactionsService } from '../../core/services/transactions.service';
import { EventLogComponent } from './components/event-log/event-log.component';
import { MerchantStatementComponent } from './components/merchant-statement/merchant-statement.component';
import { OriginalTransactionComponent } from './components/original-transaction/original-transaction.component';
import { RefundDetailsComponent } from './components/refund-details/refund-details.component';
import { StatusBannerComponent } from './components/status-banner/status-banner.component';

@Component({
  selector: 'app-transaction-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    StatusBannerComponent,
    OriginalTransactionComponent,
    RefundDetailsComponent,
    MerchantStatementComponent,
    EventLogComponent
  ],
  templateUrl: './transaction-detail.component.html',
  styleUrl: './transaction-detail.component.scss'
})
export class TransactionDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly transactionsService = inject(TransactionsService);

  readonly transaction = signal<TransactionDetail | null>(null);
  readonly loading = signal<boolean>(true);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.loadTransaction(id);
      } else {
        this.error.set('Invalid transaction reference.');
        this.loading.set(false);
      }
    });
  }

  private loadTransaction(id: string): void {
    this.loading.set(true);
    this.error.set(null);

    this.transactionsService.getTransactionById(id).subscribe({
      next: data => {
        this.transaction.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Transaction could not be found.');
        this.loading.set(false);
      }
    });
  }
}
