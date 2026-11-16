import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { TransactionDetail } from '../../core/models/transaction.model';
import { NotificationService } from '../../core/services/notification.service';
import { TransactionsService } from '../../core/services/transactions.service';

@Component({
  selector: 'app-dispute-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './dispute-form.component.html',
  styleUrl: './dispute-form.component.scss'
})
export class DisputeFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly transactionsService = inject(TransactionsService);
  private readonly notificationService = inject(NotificationService);

  readonly reasons = [
    'Unauthorized charge',
    'Duplicate charge',
    'Incorrect amount',
    'Goods not received',
    'Other'
  ];

  readonly transaction = signal<TransactionDetail | null>(null);
  readonly loading = signal<boolean>(true);
  readonly submitting = signal<boolean>(false);

  form: FormGroup = this.fb.group({
    reason: ['', Validators.required],
    description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]]
  });

  get charCount(): number {
    return this.form.get('description')?.value?.length || 0;
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.loadTransaction(id);
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.form.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  onSubmit(): void {
    if (this.form.invalid || !this.transaction()) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);
    const transactionId = this.transaction()!.id;

    this.transactionsService.createDispute(transactionId, {
      reason: this.form.value.reason,
      description: this.form.value.description.trim()
    }).subscribe({
      next: () => {
        this.submitting.set(false);
        this.notificationService.showSuccess('Dispute created successfully.');
        this.router.navigate(['/disputes']);
      },
      error: () => {
        this.submitting.set(false);
      }
    });
  }

  private loadTransaction(id: string): void {
    this.loading.set(true);
    this.transactionsService.getTransactionById(id).subscribe({
      next: data => {
        this.transaction.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }
}
